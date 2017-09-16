create database "Banking-System"
use "Banking-System"

create table UserData (
	AccountNumber numeric(9) Primary Key,
	Title varchar(4) Not Null,
	Name varchar(25) Not Null,
	TotalBalance float(15) Not Null,
	Password varchar(22) Not Null,
	LastLoginDetails smallDatetime
)

create table Passbook (
	AccountNumber numeric(9) Not Null,
	Amount float(15) Not Null,
	TimeOfTransaction smalldatetime,
	Mode varchar(25) Not Null
)

select * from Passbook
select * from UserData
