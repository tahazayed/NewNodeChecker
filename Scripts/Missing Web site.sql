
SELECT [Id]
      ,[AppPhysicalPath]
      ,[SiteName]
      ,[VirtualDirectoryName]
      ,[ServerLogId]
      ,[RowVesion]
      ,[EventDateTime]
      ,[Exception] into #LDNTRACERWEB1D
  FROM WebSiteLog
  where [ServerLogId] =29
  
  
SELECT [Id]
      ,[AppPhysicalPath]
      ,[SiteName]
      ,[VirtualDirectoryName]
      ,[ServerLogId]
      ,[RowVesion]
      ,[EventDateTime]
      ,[Exception] into #LDNTRACERWEB2D
  FROM WebSiteLog
  where [ServerLogId] =30
  
  select #LDNTRACERWEB1D.* from #LDNTRACERWEB1D
  left join #LDNTRACERWEB2D on #LDNTRACERWEB1D.[SiteName]=#LDNTRACERWEB2D.[SiteName]
and #LDNTRACERWEB1D.[VirtualDirectoryName]=#LDNTRACERWEB2D.[VirtualDirectoryName]
where #LDNTRACERWEB2D.Id is null

  select #LDNTRACERWEB2D.* from #LDNTRACERWEB2D
  left join #LDNTRACERWEB1D on #LDNTRACERWEB2D.[SiteName]=#LDNTRACERWEB1D.[SiteName]
and #LDNTRACERWEB2D.[VirtualDirectoryName]=#LDNTRACERWEB1D.[VirtualDirectoryName]
where #LDNTRACERWEB1D.Id is null



  
  drop table #LDNTRACERWEB1D,#LDNTRACERWEB2D
  
  
  

  
 --select * from InstalledAppLog where ServerLogId in(30,29) and DisplayName='Security Update for Microsoft .NET Framework 4.5 (KB2742613)' 
  
  