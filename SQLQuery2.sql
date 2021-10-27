use eShopSolution
go

select * from Products
select * from ProductTranslations
select * from ProductImages

select * from ProductTranslations

ALTER TABLE ProductTranslations
DROP CONSTRAINT FK_ProductTranslations_Languages_LanguageId1;
DROP INDEX IX_ProductTranslations_LanguageId1 ON ProductTranslations;

ALTER TABLE ProductTranslations
DROP COLUMN LanguageId1;
