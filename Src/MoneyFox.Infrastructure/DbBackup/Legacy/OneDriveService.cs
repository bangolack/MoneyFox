namespace MoneyFox.Infrastructure.DbBackup.Legacy
{

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ApplicationCore.Domain.Exceptions;
    using Core.ApplicationCore.UseCases.DbBackup;
    using Flurl.Http;
    using Microsoft.Graph;
    using Microsoft.Identity.Client;
    using MoneyFox.Infrastructure.DbBackup.OneDriveModels;

    internal class OneDriveService : IOneDriveBackupService
    {
        private const string ARCHIVE_FOLDER_NAME = "Archive";
        private const string BACKUP_NAME_TEMPLATE = "backupmoneyfox3_{0}.db";
        private const int BACKUP_ARCHIVE_COUNT = 15;
        private const string ERROR_CODE_CANCELED = "authentication_canceled";

        private readonly IOneDriveAuthenticationService oneDriveAuthenticationService;

        public OneDriveService(IOneDriveAuthenticationService oneDriveAuthenticationService)
        {
            this.oneDriveAuthenticationService = oneDriveAuthenticationService;
        }

        public async Task LoginAsync()
        {
            await oneDriveAuthenticationService.AcquireAuthentication();
        }

        public async Task LogoutAsync()
        {
            await oneDriveAuthenticationService.LogoutAsync();
        }

        public async Task<bool> UploadAsync(Stream dataToUpload)
        {
            var graphServiceClient = await oneDriveAuthenticationService.CreateServiceClient();
            try
            {
                var backupName = string.Format(format: BACKUP_NAME_TEMPLATE, arg0: DateTime.UtcNow.ToString(format: "yyyy-M-d_hh-mm-ssss"));
                var uploadedItem = await graphServiceClient.Me.Drive.Special.AppRoot.ItemWithPath(backupName)
                    .Content.Request()
                    .PutAsync<DriveItem>(dataToUpload);

                await CleanupOldBackupsAsync(graphServiceClient);
                await DeleteExistingFolderAsync(graphServiceClient);

                return uploadedItem != null;
            }
            catch (MsalClientException ex)
            {
                if (ex.ErrorCode == ERROR_CODE_CANCELED)
                {
                    throw new BackupOperationCanceledException(ex);
                }

                throw;
            }
            catch (OperationCanceledException ex)
            {
                throw new BackupOperationCanceledException(ex);
            }
            catch (Exception ex)
            {
                throw new BackupAuthenticationFailedException(ex);
            }
        }

        public async Task<DateTime> GetBackupDateAsync()
        {
            var graphServiceClient = await oneDriveAuthenticationService.CreateServiceClient();
            var existingBackups = await graphServiceClient.Me.Drive.Special.AppRoot.Children.Request().GetAsync();
            if (existingBackups.Any())
            {
                return existingBackups.OrderByDescending(di => di.LastModifiedDateTime).First().LastModifiedDateTime?.DateTime.ToLocalTime()
                       ?? DateTime.MinValue;
            }

            return DateTime.MinValue;
        }

        public async Task<List<string>> GetFileNamesAsync()
        {
            try
            {
                var graphServiceClient = await oneDriveAuthenticationService.CreateServiceClient();

                return (await graphServiceClient.Me.Drive.Special.AppRoot.Children.Request().GetAsync()).Select(x => x.Name).ToList();
            }
            catch (Exception ex)
            {
                throw new BackupAuthenticationFailedException(ex);
            }
        }

        public async Task<Stream> RestoreAsync()
        {
            try
            {
                var graphServiceClient = await oneDriveAuthenticationService.CreateServiceClient();
                var existingBackups = await graphServiceClient.Me.Drive.Special.AppRoot.Children.Request().GetAsync();
                if (existingBackups.Any() is false)
                {
                    throw new NoBackupFoundException();
                }

                var lastBackup = existingBackups.OrderByDescending(di => di.LastModifiedDateTime).First();

                return await graphServiceClient.Drive.Items[lastBackup.Id].Content.Request().GetAsync();
            }
            catch (MsalClientException ex)
            {
                if (ex.ErrorCode == ERROR_CODE_CANCELED)
                {
                    throw new BackupOperationCanceledException();
                }

                throw;
            }
            catch (Exception ex)
            {
                throw new BackupAuthenticationFailedException(ex);
            }
        }

        public async Task<UserAccountDto> GetUserAccountAsync()
        {
            var authentication = await oneDriveAuthenticationService.AcquireAuthentication();
            var userDto = await "https://graph.microsoft.com/v1.0/me"
                .WithOAuthBearerToken(authentication.AccessToken)
                .GetJsonAsync<UserDto>();

            return new UserAccountDto(name: userDto.DisplayName, email: userDto.PrincipalName);
        }

        private static async Task CleanupOldBackupsAsync(GraphServiceClient graphServiceClient)
        {
            var existingBackups = await graphServiceClient.Me.Drive.Special.AppRoot.Children.Request().GetAsync();
            if (existingBackups.Count < BACKUP_ARCHIVE_COUNT)
            {
                return;
            }

            var oldestBackup = existingBackups.OrderByDescending(x => x.CreatedDateTime).Last();
            await graphServiceClient.Drive.Items[oldestBackup?.Id].Request().DeleteAsync();
        }

        private static async Task DeleteExistingFolderAsync(GraphServiceClient graphServiceClient)
        {
            var archiveFolder = (await graphServiceClient.Me.Drive.Special.AppRoot.Children.Request().GetAsync()).CurrentPage.FirstOrDefault(
                x => x.Name == ARCHIVE_FOLDER_NAME);

            if (archiveFolder != null)
            {
                await graphServiceClient.Me.Drive.Items[archiveFolder.Id].Request().DeleteAsync();
            }
        }
    }

}
