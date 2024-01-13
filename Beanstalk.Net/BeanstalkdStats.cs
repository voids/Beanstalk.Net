using YamlDotNet.Serialization;

namespace Beanstalk.Net {
    /// <summary>
    /// The stats data for the system is a YAML file representing a single dictionary of string keys to scalar values.
    /// Entries described as "cumulative" are reset when the beanstalkd process starts; they are not stored on disk with the -b flag.
    /// </summary>
    public class BeanstalkdStats {
        /// <summary>
        /// The number of ready jobs with priority &lt; 1024.
        /// </summary>
        [YamlMember(Alias = "current-jobs-urgent")]
        public uint CurrentJobsUrgent { get; set; }

        /// <summary>
        /// The number of jobs in the ready queue.
        /// </summary>
        [YamlMember(Alias = "current-jobs-ready")]
        public uint CurrentJobsReady { get; set; }

        /// <summary>
        /// The number of jobs reserved by all clients.
        /// </summary>
        [YamlMember(Alias = "current-jobs-reserved")]
        public uint CurrentJobsReserved { get; set; }

        /// <summary>
        /// The number of delayed jobs.
        /// </summary>
        [YamlMember(Alias = "current-jobs-delayed")]
        public uint CurrentJobsDelayed { get; set; }

        /// <summary>
        /// the number of buried jobs.
        /// </summary>
        [YamlMember(Alias = "current-jobs-buried")]
        public uint CurrentJobsBuried { get; set; }

        /// <summary>
        /// the cumulative number of put commands.
        /// </summary>
        [YamlMember(Alias = "cmd-put")]
        public uint CmdPut { get; set; }

        /// the cumulative number of peek commands.
        [YamlMember(Alias = "cmd-peek")]
        public uint CmdPeek { get; set; }

        /// <summary>
        /// the cumulative number of peek-ready commands.
        /// </summary>
        [YamlMember(Alias = "cmd-peek-ready")]
        public uint CmdPeekReady { get; set; }

        /// <summary>
        /// the cumulative number of peek-delayed commands.
        /// </summary>
        [YamlMember(Alias = "cmd-peek-delayed")]
        public uint CmdPeekDelayed { get; set; }

        /// <summary>
        /// the cumulative number of peek-buried commands.
        /// </summary>
        [YamlMember(Alias = "cmd-peek-buried")]
        public uint CmdPeekBuried { get; set; }

        /// <summary>
        /// the cumulative number of reserve commands.
        /// </summary>
        [YamlMember(Alias = "cmd-reserve")]
        public uint CmdReserve { get; set; }

        /// <summary>
        /// the cumulative number of reserve-with-timeout commands.
        /// </summary>
        [YamlMember(Alias = "cmd-reserve-with-timeout")]
        public uint CmdReserveWithTimeout { get; set; }

        /// <summary>
        /// the cumulative number of touch commands.
        /// </summary>
        [YamlMember(Alias = "cmd-touch")]
        public uint CmdTouch { get; set; }

        /// <summary>
        /// the cumulative number of use commands.
        /// </summary>
        [YamlMember(Alias = "cmd-use")]
        public uint CmdUse { get; set; }

        /// <summary>
        /// the cumulative number of watch commands.
        /// </summary>
        [YamlMember(Alias = "cmd-watch")]
        public uint CmdWatch { get; set; }

        /// <summary>
        /// the cumulative number of ignore commands.
        /// </summary>
        [YamlMember(Alias = "cmd-ignore")]
        public uint CmdIgnore { get; set; }

        /// <summary>
        /// the cumulative number of delete commands.
        /// </summary>
        [YamlMember(Alias = "cmd-delete")]
        public uint CmdDelete { get; set; }

        /// <summary>
        /// the cumulative number of release commands.
        /// </summary>
        [YamlMember(Alias = "cmd-release")]
        public uint CmdRelease { get; set; }

        /// <summary>
        /// the cumulative number of bury commands.
        /// </summary>
        [YamlMember(Alias = "cmd-bury")]
        public uint CmdBury { get; set; }

        /// <summary>
        /// the cumulative number of kick commands.
        /// </summary>
        [YamlMember(Alias = "cmd-kick")]
        public uint CmdKick { get; set; }

        /// <summary>
        /// the cumulative number of stats commands.
        /// </summary>
        [YamlMember(Alias = "cmd-stats")]
        public uint CmdStats { get; set; }

        /// <summary>
        /// the cumulative number of stats-job commands.
        /// </summary>
        [YamlMember(Alias = "cmd-stats-job")]
        public uint CmdStatsJob { get; set; }

        /// <summary>
        /// the cumulative number of stats-tube commands.
        /// </summary>
        [YamlMember(Alias = "cmd-stats-tube")]
        public uint CmdStatsTube { get; set; }

        /// <summary>
        /// the cumulative number of list-tubes commands.
        /// </summary>
        [YamlMember(Alias = "cmd-list-tubes")]
        public uint CmdListTubes { get; set; }

        /// <summary>
        /// the cumulative number of list-tube-used commands.
        /// </summary>
        [YamlMember(Alias = "cmd-list-tube-used")]
        public uint CmdListTubeUsed { get; set; }

        /// <summary>
        /// the cumulative number of list-tubes-watched commands.
        /// </summary>
        [YamlMember(Alias = "cmd-list-tubes-watched")]
        public uint CmdListTubesWatched { get; set; }

        /// <summary>
        /// the cumulative number of pause-tube commands.
        /// </summary>
        [YamlMember(Alias = "cmd-pause-tube")]
        public uint CmdPauseTube { get; set; }

        /// <summary>
        /// the cumulative count of times a job has timed out.
        /// </summary>
        [YamlMember(Alias = "job-timeouts")]
        public uint JobTimeouts { get; set; }

        /// <summary>
        /// the cumulative count of jobs created.
        /// </summary>
        [YamlMember(Alias = "total-jobs")]
        public uint TotalJobs { get; set; }

        /// <summary>
        /// the maximum number of bytes in a job.
        /// </summary>
        [YamlMember(Alias = "max-job-size")]
        public uint MaxJobSize { get; set; }

        /// <summary>
        /// the number of currently-existing tubes.
        /// </summary>
        [YamlMember(Alias = "current-tubes")]
        public uint CurrentTubes { get; set; }

        /// <summary>
        /// the number of currently open connections.
        /// </summary>
        [YamlMember(Alias = "current-connections")]
        public uint CurrentConnections { get; set; }

        /// <summary>
        /// the number of open connections that have each issued at least one put command.
        /// </summary>
        [YamlMember(Alias = "current-producers")]
        public uint CurrentProducers { get; set; }

        /// <summary>
        /// the number of open connections that have each issued at least one reserve command.
        /// </summary>
        [YamlMember(Alias = "current-workers")]
        public uint CurrentWorkers { get; set; }

        /// <summary>
        /// the number of open connections that have issued a reserve command but not yet received a response.
        /// </summary>
        [YamlMember(Alias = "current-waiting")]
        public uint CurrentWaiting { get; set; }

        /// <summary>
        /// the cumulative count of connections.
        /// </summary>
        [YamlMember(Alias = "total-connections")]
        public uint TotalConnections { get; set; }

        /// <summary>
        /// the process id of the server.
        /// </summary>
        [YamlMember(Alias = "pid")]
        public long Pid { get; set; }

        /// <summary>
        /// the version string of the server.
        /// </summary>
        [YamlMember(Alias = "version")]
        public string Version { get; set; }

        /// <summary>
        /// the cumulative user CPU time of this process in seconds and microseconds.
        /// </summary>
        [YamlMember(Alias = "rusage-utime")]
        public double RUsageUTime { get; set; }

        /// <summary>
        /// the cumulative system CPU time of this process in seconds and microseconds.
        /// </summary>
        [YamlMember(Alias = "rusage-stime")]
        public double RUsageSTime { get; set; }

        /// <summary>
        /// the number of seconds since this server process started running.
        /// </summary>
        [YamlMember(Alias = "uptime")]
        public uint Uptime { get; set; }

        /// <summary>
        /// the index of the oldest binlog file needed to store the current jobs.
        /// </summary>
        [YamlMember(Alias = "binlog-oldest-index")]
        public uint BinlogOldestIndex { get; set; }

        /// <summary>
        /// the index of the current binlog file being written to. If binlog is not active this value will be 0.
        /// </summary>
        [YamlMember(Alias = "binlog-current-index")]
        public uint BinlogCurrentIndex { get; set; }

        /// <summary>
        /// the maximum size in bytes a binlog file is allowed to get before a new binlog file is opened.
        /// </summary>
        [YamlMember(Alias = "binlog-max-size")]
        public uint BinlogMaxSize { get; set; }

        /// <summary>
        /// the cumulative number of records written to the binlog.
        /// </summary>
        [YamlMember(Alias = "binlog-records-written")]
        public uint BinlogRecordsWritten { get; set; }

        /// <summary>
        /// the cumulative number of records written as part of compaction.
        /// </summary>
        [YamlMember(Alias = "binlog-records-migrated")]
        public uint BinlogRecordsMigrated { get; set; }

        /// <summary>
        /// "draining" is set to "true" if the server is in drain mode, "false" otherwise.
        /// </summary>
        [YamlMember(Alias = "draining")]
        public bool Draining { get; set; }

        /// <summary>
        /// a random id string for this server process, generated every time beanstalkd process starts.
        /// </summary>
        [YamlMember(Alias = "id")]
        public string Id { get; set; }

        /// <summary>
        /// the hostname of the machine as determined by uname.
        /// </summary>
        [YamlMember(Alias = "hostname")]
        public string Hostname { get; set; }

        /// <summary>
        /// OS version as determined by uname
        /// </summary>
        [YamlMember(Alias = "os")]
        public string Os { get; set; }

        /// <summary>
        /// the machine architecture as determined by uname
        /// </summary>
        [YamlMember(Alias = "platform")]
        public string Platform { get; set; }
    }
}