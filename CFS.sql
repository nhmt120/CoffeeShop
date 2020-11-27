CREATE DATABASE CFS
USE CFS

/*
use ATM
drop database CFS
*/


-- identity = auto increment
-- drop table accounts
CREATE TABLE Accounts (
	account_id int IDENTITY(1, 1) PRIMARY KEY,
	username varchar(25) NOT NULL,
	[password] varchar(25) NOT NULL,
	[name] nvarchar(25),
	[role] varchar(25) DEFAULT 'Cashier'
)

INSERT INTO Accounts (username, password, name, role)
VALUES ('vince', '123456', 'Vince', default),
	   ('admin', 'admin', 'Administrator', 'Admin')

-- select * from Accounts

