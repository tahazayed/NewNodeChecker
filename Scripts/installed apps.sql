
Declare @ServerLogIdLDNTRACERWEB1D int, @ServerLogIdLDNTRACERWEB2D int 
set @ServerLogIdLDNTRACERWEB1D = 39
set @ServerLogIdLDNTRACERWEB2D = 40
SELECT [Id]
      ,[DisplayName]
      ,isnull([DisplayVersion],0) as [DisplayVersion]
      ,[InstallDate]
      ,[InstallSource]
      ,[ServerLogId]
      ,[RowVesion]
      ,[EventDateTime]
      ,[Exception] into #LDNTRACERWEB1D
  FROM InstalledAppLog
  where [ServerLogId] = @ServerLogIdLDNTRACERWEB1D
  
  
SELECT [Id]
      ,[DisplayName]
      ,isnull([DisplayVersion],0) as [DisplayVersion]
      ,[InstallDate]
      ,[InstallSource]
      ,[ServerLogId]
      ,[RowVesion]
      ,[EventDateTime]
      ,[Exception] into #LDNTRACERWEB2D
  FROM InstalledAppLog
  where [ServerLogId] =@ServerLogIdLDNTRACERWEB2D
  
  select #LDNTRACERWEB1D.* from #LDNTRACERWEB1D
  left join #LDNTRACERWEB2D on #LDNTRACERWEB1D.DisplayName=#LDNTRACERWEB2D.DisplayName
and #LDNTRACERWEB1D.DisplayVersion=#LDNTRACERWEB2D.DisplayVersion
where #LDNTRACERWEB2D.Id is null

  select #LDNTRACERWEB2D.* from #LDNTRACERWEB2D
  left join #LDNTRACERWEB1D on #LDNTRACERWEB2D.DisplayName=#LDNTRACERWEB1D.DisplayName
and #LDNTRACERWEB2D.DisplayVersion=#LDNTRACERWEB1D.DisplayVersion
where #LDNTRACERWEB1D.Id is null



  
  drop table #LDNTRACERWEB1D,#LDNTRACERWEB2D
  
  
  

  
 --select * from InstalledAppLog where ServerLogId in(@ServerLogIdLDNTRACERWEB2D,@ServerLogIdLDNTRACERWEB1D) and DisplayName='Security Update for Microsoft .NET Framework 4.5 (KB2742613)' 
  
  