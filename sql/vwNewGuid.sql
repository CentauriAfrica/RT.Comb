SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW vwNewGuid AS SELECT NEWID() AS id
GO