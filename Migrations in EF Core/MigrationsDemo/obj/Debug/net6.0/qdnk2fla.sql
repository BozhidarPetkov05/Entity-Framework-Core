BEGIN TRANSACTION;
GO

ALTER TABLE [Students] ADD [Address] nvarchar(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240717150729_AddressAdded', N'6.0.31');
GO

COMMIT;
GO

