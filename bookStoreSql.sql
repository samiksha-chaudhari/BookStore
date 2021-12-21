create database BookStore;
use BookStore;
create table RegisterUser
(
 UserId int  primary key,
 UserName varchar(100) not null,
 Email varchar(100) not null,
 PhoneNo varchar(12) not null,
 Password varchar(20) not null
);
