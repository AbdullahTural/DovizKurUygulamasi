create database Doviz
go
use Doviz
go
create table ParaBirimi
(
ID uniqueidentifier primary key ,
Code nvarchar(8),
Tanim nvarchar(70),
UyariLimit decimal -- 2,33< x > 2,43
)
go
insert into ParaBirimi (ID , Code,Tanim,UyariLimit) values (newid() , 'USD' , 'Amarikan Dolar�' , 4.25)
insert into ParaBirimi (ID , Code,Tanim,UyariLimit) values (newid() , 'EUR' , 'EURO' , 0)
insert into ParaBirimi (ID , Code,Tanim,UyariLimit) values (newid() , 'GBP' , '�ngiliz Sterlini' , 0)

select * from ParaBirimi
go 

create table Kur(
 ID uniqueidentifier primary key ,
 ParaBirimiID uniqueidentifier,
 Alis decimal , 
 Satis decimal,
 OlusturmaTarih datetime 
)

create table KurGecmis
(
 ID uniqueidentifier primary key ,
 KurID uniqueidentifier,
 ParaBirimiID uniqueidentifier,
 Alis decimal , 
 Satis decimal,
 OlusturmaTarih datetime 
)

--create proc KurKayitEKLE
--(
--@ID uniqueidentifier ,
--@ParaBirimiID uniqueidentifier,
--@Alis decimal , 
--@Satis decimal,
--@OlusturmaTarih datetime )
-- as
-- begin
-- if((select count (*) from kur where ParaBirimiID = @ParaBirimiID ) > 0 )
-- begin
-- -- Kur tablosundaki mevcut kayd� kurgecmis tablosuna aktarmam�z gerekiyor.
-- insert into KurGecmis (ID,KurID,ParaBirimiID,Alis,Satis,OlusturmaTarih) select newid() , ID,ParaBirimiID,Alis,Satis ,OlusturmaTarih from 
-- Kur where ParaBirimiID = @ParaBirimiID
-- -- Kur tablosundaki de�eri g�ncelleyelim .

-- update Kur Set 
-- Alis =@Alis,
-- Satis = @Satis
-- where
-- ParaBirimiID = @ParaBirimiID
-- end
-- else
-- begin
-- insert into Kur (ID , ParaBirimiID , Alis , Satis , OlusturmaTarih) values (@ID , @ParaBirimiID, @Alis,@Satis,@OlusturmaTarih)
-- end
-- end
--)