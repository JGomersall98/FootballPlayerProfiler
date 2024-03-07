CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `Fixtures` (
    `FixtureId` int NOT NULL AUTO_INCREMENT,
    `City` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Date` datetime(6) NOT NULL,
    `Time` time(6) NOT NULL,
    `Temperature` double NOT NULL,
    CONSTRAINT `PK_Fixtures` PRIMARY KEY (`FixtureId`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Players` (
    `PlayerId` int NOT NULL AUTO_INCREMENT,
    `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Photo` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Position` longtext CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Players` PRIMARY KEY (`PlayerId`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `PlayerStats` (
    `StatId` int NOT NULL AUTO_INCREMENT,
    `FixtureId` int NOT NULL,
    `PlayerId` int NOT NULL,
    `Rating` double NOT NULL,
    `PassingAccuracy` int NOT NULL,
    `TotalTackles` int NOT NULL,
    `FoulsCommitted` int NOT NULL,
    `DuelsTotal` int NOT NULL,
    `DuelsWon` int NOT NULL,
    `DribblingAttempts` int NOT NULL,
    `DribblingSuccess` int NOT NULL,
    `TotalShots` int NOT NULL,
    `ShotsOnTarget` int NOT NULL,
    CONSTRAINT `PK_PlayerStats` PRIMARY KEY (`StatId`),
    CONSTRAINT `FK_PlayerStats_Fixtures_FixtureId` FOREIGN KEY (`FixtureId`) REFERENCES `Fixtures` (`FixtureId`) ON DELETE CASCADE,
    CONSTRAINT `FK_PlayerStats_Players_PlayerId` FOREIGN KEY (`PlayerId`) REFERENCES `Players` (`PlayerId`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_PlayerStats_FixtureId` ON `PlayerStats` (`FixtureId`);

CREATE INDEX `IX_PlayerStats_PlayerId` ON `PlayerStats` (`PlayerId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240226090900_addedPositionToPlayersTable', '7.0.10');

COMMIT;

START TRANSACTION;

ALTER TABLE `Fixtures` DROP COLUMN `City`;

ALTER TABLE `Fixtures` ADD `CityId` int NOT NULL DEFAULT 0;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240226115631_ChangeCityToCityID', '7.0.10');

COMMIT;

START TRANSACTION;

ALTER TABLE `Fixtures` DROP COLUMN `CityId`;

ALTER TABLE `Fixtures` ADD `PlaceId` longtext CHARACTER SET utf8mb4 NOT NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240226120214_PlaceIdIsString', '7.0.10');

COMMIT;

