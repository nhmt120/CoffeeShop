CREATE DATABASE CFS
USE CFS

/*
create database foo
use foo
drop database CFS
drop database foo
*/


-- identity = auto increment
CREATE TABLE Accounts (
	account_id int IDENTITY(1, 1) PRIMARY KEY,
	username varchar(25) NOT NULL,
	[password] varchar(25) NOT NULL,
	[name] nvarchar(25),
	[role] varchar(25)
)

INSERT INTO Accounts (username, password, name, role)
VALUES ('vince', 'vince', 'Vince', 'Cashier'),
	   ('admin', 'admin', 'Administrator', 'Admin'),
	   ('cashier', 'cashier', 'Cashier', 'Cashier')

-- drop table accounts
-- select * from Accounts
-- where role = 'Admin'
-- "SELECT * FROM Accounts WHERE username = 'cashier1' and password = 'cashier1';"

CREATE TABLE Drinks (
	drink_id int IDENTITY(1, 1) PRIMARY KEY,
	[name] nvarchar(25) NOT NULL,
	price money NOT NULL,
	stock int DEFAULT 10,
	[image] nvarchar(250) NULL
)

INSERT INTO Drinks (name, price)
VALUES ('Espresso', 3.0),
	   ('Ristretto', 3.5),
	   ('Doppio', 3.5),
	   ('Latte', 4.0),
	   ('Cappuchino', 4.0),
	   ('Macchiato', 4.0),
	   ('Americano', 2.5),
	   ('Cold Brew', 3.0)

-- drop table Drinks
-- select * from Drinks where name = 'Hot Choc'


CREATE TABLE Orders (
	order_id int IDENTITY(1, 1) PRIMARY KEY,
	total money NOT NULL,
	[date] date
)

CREATE TABLE Order_Details (
	order_id int FOREIGN KEY REFERENCES Orders(order_id),
	drink_name nvarchar(25)
)
