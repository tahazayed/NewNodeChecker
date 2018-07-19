
Declare @ServerLogIdLDNTRACERWEB1D int, @ServerLogIdLDNTRACERWEB2D int 
set @ServerLogIdLDNTRACERWEB1D = 37
set @ServerLogIdLDNTRACERWEB2D = 38
update WebSiteFileLog set BuildNo='' where BuildNo is null
SELECT [Id]
      ,[AppPhysicalPath]
      ,[SiteName]
      ,[VirtualDirectoryName]
      ,[ServerLogId]
      ,[EventDateTime]
      ,[Exception] into #LDNTRACERWEB1D
  FROM WebSiteLog
  where [ServerLogId] =@ServerLogIdLDNTRACERWEB1D
  
  
SELECT [Id]
      ,[AppPhysicalPath]
      ,[SiteName]
      ,[VirtualDirectoryName]
      ,[ServerLogId]
      ,[EventDateTime]
      ,[Exception] into #LDNTRACERWEB2D
  FROM WebSiteLog
  where [ServerLogId] =@ServerLogIdLDNTRACERWEB2D
  
  select #LDNTRACERWEB1D.* into #deleteIds from #LDNTRACERWEB1D
  left join #LDNTRACERWEB2D on #LDNTRACERWEB1D.[SiteName]=#LDNTRACERWEB2D.[SiteName]
and #LDNTRACERWEB1D.[VirtualDirectoryName]=#LDNTRACERWEB2D.[VirtualDirectoryName]
where #LDNTRACERWEB2D.Id is null

insert into #deleteIds
  select #LDNTRACERWEB2D.* from #LDNTRACERWEB2D
  left join #LDNTRACERWEB1D on #LDNTRACERWEB2D.[SiteName]=#LDNTRACERWEB1D.[SiteName]
and #LDNTRACERWEB2D.[VirtualDirectoryName]=#LDNTRACERWEB1D.[VirtualDirectoryName]
where #LDNTRACERWEB1D.Id is null

delete from #LDNTRACERWEB1D where ID in(select ID from #deleteIds)
delete from #LDNTRACERWEB2D where ID in(select ID from #deleteIds)


  select distinct #LDNTRACERWEB1D.*  from #LDNTRACERWEB1D
  inner join #LDNTRACERWEB2D on #LDNTRACERWEB1D.[SiteName]=#LDNTRACERWEB2D.[SiteName]
and #LDNTRACERWEB1D.[VirtualDirectoryName]=#LDNTRACERWEB2D.[VirtualDirectoryName]
inner join WebSiteFileLog as WebSiteFileLog1D on WebSiteFileLog1D.WebSiteLogId=#LDNTRACERWEB1D.Id
inner join WebSiteFileLog as WebSiteFileLog2D on WebSiteFileLog2D.WebSiteLogId=#LDNTRACERWEB2D.Id
where WebSiteFileLog1D.PhysicalPath=WebSiteFileLog2D.PhysicalPath
and WebSiteFileLog1D.FileName=WebSiteFileLog2D.FileName
and
(WebSiteFileLog1D.Size<>WebSiteFileLog2D.Size
or WebSiteFileLog1D.BuildNo<>WebSiteFileLog2D.BuildNo
or WebSiteFileLog1D.LastModificationDate<>WebSiteFileLog2D.LastModificationDate
)
  
drop table #LDNTRACERWEB1D,#LDNTRACERWEB2D,#deleteIds
  
  


  
 --select * from InstalledAppLog where ServerLogId in(@ServerLogIdLDNTRACERWEB2D,@ServerLogIdLDNTRACERWEB1D) and DisplayName='Security Update for Microsoft .NET Framework 4.5 (KB2742613)' 
  
  