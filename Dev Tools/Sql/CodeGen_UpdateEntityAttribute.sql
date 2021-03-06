/* Code Generate 'UpdateEntityAttribute(...)' for migrations. 
*/
DECLARE @entityTypeNameSearch NVARCHAR(MAX) = '%GroupType%' -- < Change this
DECLARE @entityTypeQualifierValueSearch NVARCHAR(MAX) = '%GroupTypePurpose%' -- < Change this 
DECLARE @daysAgo INT = NULL --< Change this to how far back to look for newly added attributes (or null to not limit)

DECLARE @crlf varchar(2) = char(13) + char(10)

SELECT 
	  '            // Entity: ' + [e].[Name] + ' Attribute: ' + [a].[Name] + @crlf
	+ '            RockMigrationHelper.AddOrUpdateEntityAttribute( ' +    
	+ '"' + [e].[Name] + '", '											-- Entity.Name
	+ '"' + CONVERT(NVARCHAR(50), [ft].[Guid]) + '", '					-- FieldType.Guid
	+ '"' + [a].[EntityTypeQualifierColumn] + '", '						-- Attribute.EntityTypeQualifierColumn
	+ '"' + [a].[EntityTypeQualifierValue] + '", '						-- Attribute.EntityTypeQualifierValue
	+ '"' + [a].[Name] + '", '											-- Attribute.Name
	+ '"' + ISNULL([a].[AbbreviatedName], '') + '", '					-- Attribute.AbbreviatedName
	+ '@"'+ ISNULL(REPLACE([a].[Description], '"', '""'),'') + '", '	-- Attribute.Description
	+ CONVERT(VARCHAR, [a].[Order]) + ', '								-- Attribute.Order
	+ '@"'+ ISNULL(REPLACE([a].[DefaultValue], '"', '""'),'') + '", '	-- Attribute.DefaultValue
	+ '"' + CONVERT(NVARCHAR(50), [a].[Guid]) + '", '					-- Attribute.Guid
	+ '"' + [a].[Key] + '");'											-- Attribute.Key
	+ @crlf AS [Up.AddEntityAttribute]
FROM [Attribute] [a]
INNER JOIN [EntityType] [e] on [e].Id = [a].EntityTypeId
INNER JOIN [FieldType] [ft] on [ft].Id = [a].FieldTypeId
WHERE [e].[Name] LIKE @entityTypeNameSearch
	AND [a].[EntityTypeQualifierColumn] LIKE @entityTypeQualifierValueSearch
	AND (([a].[CreatedDateTime] IS NOT NULL AND DATEDIFF(HOUR, [a].[CreatedDateTime], SYSDATETIME()) < (@daysAgo * 24)) OR @daysAgo IS NULL)
ORDER BY [e].[name], [a].[Order] 

SELECT 
		  '            // Qualifier for attribute: ' + [a].[Key] + @crlf
		+ '            RockMigrationHelper.UpdateAttributeQualifier( '
		+ '"' + CONVERT(NVARCHAR(50), [a].[Guid]) + '", '
        + '"' + [aq].[Key] + '", '
		+ '@"' + [aq].[Value] + '", '
        + '"' + CONVERT(NVARCHAR(50), [aq].[Guid]) + '");'
		+ @crlf AS [Up.UpdateAttributeQualifier]
  FROM [Attribute] [a]
  INNER JOIN [AttributeQualifier] [aq] ON [aq].AttributeId = [a].[Id]
  INNER JOIN [EntityType] [e] ON [e].[Id] = [a].[EntityTypeId]
  INNER JOIN [FieldType] [ft] ON [ft].[Id] = [a].[FieldTypeId]
  WHERE [e].[Name] LIKE @entityTypeNameSearch
	AND [a].[EntityTypeQualifierColumn] like @entityTypeQualifierValueSearch
	AND (([a].[CreatedDateTime] IS NOT NULL AND DATEDIFF(HOUR, [a].[CreatedDateTime], SYSDATETIME()) < (@daysAgo * 24)) OR @daysAgo IS NULL)
ORDER BY [e].[name], [a].[Order]

SELECT
	  '            // Attribute: ' + [a].[Name] + @crlf
    + '            RockMigrationHelper.AddAttributeValue( '     
    + '"' + CONVERT(NVARCHAR(50), [a].[Guid]) + '", '
	+ '0, '
    + '@"'+ ISNULL([av].[Value],'') + '", '
    + '"' + CONVERT(NVARCHAR(50), [a].[Guid]) + '");'
    + @crlf AS [Up.AddAttributeValue]
FROM [AttributeValue] [av]
JOIN [Attribute] [a] ON [a].[Id] = [av].[AttributeId]
INNER JOIN [EntityType] [e] ON [e].[Id] = [a].[EntityTypeId]
INNER JOIN [FieldType] [ft] ON [ft].[Id] = [a].[FieldTypeId]
WHERE [e].[Name] LIKE @entityTypeNameSearch
	AND [a].[EntityTypeQualifierColumn] LIKE @entityTypeQualifierValueSearch
	AND (([a].[CreatedDateTime] IS NOT NULL AND DATEDIFF(HOUR, [a].[CreatedDateTime], SYSDATETIME()) < (@daysAgo * 24)) OR @daysAgo IS NULL)
ORDER BY [e].[Name], [a].[Order] 

SELECT 
      '            RockMigrationHelper.DeleteAttribute( ' +    
    + '"' + CONVERT(NVARCHAR(50), [a].[Guid]) + '"); ' 
	+ '// ' + [e].[Name] + ': '
	+ [a].[Name]
	+ @crlf AS [Down.DeleteAttribute]
FROM [Attribute] [a]
INNER JOIN [EntityType] [e] ON [e].[Id] = [a].[EntityTypeId]
INNER JOIN [FieldType] [ft] ON [ft].[Id] = [a].[FieldTypeId]
WHERE [e].[Name] LIKE @entityTypeNameSearch
	AND [a].[EntityTypeQualifierColumn] LIKE @entityTypeQualifierValueSearch
	AND (([a].[CreatedDateTime] IS NOT NULL AND DATEDIFF(HOUR, [a].[CreatedDateTime], SYSDATETIME()) < (@daysAgo * 24)) OR @daysAgo IS NULL)
ORDER BY [e].[Name], [a].[Order] 
