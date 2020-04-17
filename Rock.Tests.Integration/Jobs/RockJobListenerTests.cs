using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using Rock.Data;
using Rock.Jobs;
using Rock.Model;
using Rock.Tests.Shared;

namespace Rock.Tests.Integration.Jobs
{
    [TestClass]
    public class RockJobListenerTests
    {
        [TestCleanup]
        public void TestCleanup()
        {
            RemoveTestJob();
        }

        [TestMethod]
        public void RockJobListnerShouldHandleExceptionCorrectly()
        {
            var expectedExceptionMessage = $"{Guid.NewGuid()} Rock Job Listener Exception Message";

            RunJob( GetJobDataMapDictionary( TestResultType.Exception, expectedExceptionMessage ) );

            var actualJob = GetAddTestJob();

            Assert.That.AreEqual( expectedExceptionMessage, actualJob.LastStatusMessage );
            Assert.That.AreEqual( "Exception", actualJob.LastStatus );

            var exceptions = new ExceptionLogService( new RockContext() ).Queryable().Where( els => els.Description == expectedExceptionMessage );
            Assert.That.IsTrue( exceptions.Count() == 1 );
        }

        [TestMethod]
        public void RockJobListnerShouldHandleMultipleAggregateExceptionCorrectly()
        {
            var expectedExceptionMessage = $"{Guid.NewGuid()} Rock Job Listener Multiple Aggregate Exception Message";

            RunJob( GetJobDataMapDictionary( TestResultType.MultipleAggregateException, expectedExceptionMessage ) );
            
            var actualJob = GetAddTestJob();

            Assert.That.AreEqual( $"One or more exceptions occurred. First Exception: {expectedExceptionMessage} 1", actualJob.LastStatusMessage );
            Assert.That.AreEqual( "Exception", actualJob.LastStatus );

            var exceptions = new ExceptionLogService( new RockContext() ).Queryable().Where( els => els.Description.Contains(expectedExceptionMessage) );
            Assert.That.IsTrue( exceptions.Count() > 1 );
        }

        [TestMethod]
        public void RockJobListnerShouldHandleSingleAggregateExceptionCorrectly()
        {
            var expectedExceptionMessage = $"{Guid.NewGuid()} Rock Job Listener Single Aggregate Exception Message";

            RunJob( GetJobDataMapDictionary( TestResultType.SingleAggregateException, expectedExceptionMessage ) );

            var actualJob = GetAddTestJob();

            Assert.That.AreEqual( expectedExceptionMessage, actualJob.LastStatusMessage );
            Assert.That.AreEqual( "Exception", actualJob.LastStatus );

            var exceptions = new ExceptionLogService( new RockContext() ).Queryable().Where( els => els.Description == expectedExceptionMessage );
            Assert.That.IsTrue( exceptions.Count() == 1 );
        }

        [TestMethod]
        public void RockJobListnerShouldHandleWarningExceptionCorrectly()
        {
            var expectedResultMessage = $"{Guid.NewGuid()} Rock Job Listener Completed With Warnings";
            var expectedExceptionsCount = new ExceptionLogService( new RockContext() ).Queryable().Count();

            RunJob( GetJobDataMapDictionary( TestResultType.Warning, expectedResultMessage ) );

            var actualExceptionsCount = new ExceptionLogService( new RockContext() ).Queryable().Count();

            var actualJob = GetAddTestJob();

            Assert.That.AreEqual( expectedResultMessage, actualJob.LastStatusMessage );
            Assert.That.AreEqual( "Warning", actualJob.LastStatus );

            Assert.That.AreEqual( expectedExceptionsCount, actualExceptionsCount );
        }

        [TestMethod]
        public void RockJobListnerShouldHandleSuccessCorrectly()
        {
            var expectedResultMessage = $"{Guid.NewGuid()} Rock Job Listener Success!";
            var expectedExceptionsCount = new ExceptionLogService( new RockContext() ).Queryable().Count();

            RunJob( GetJobDataMapDictionary( TestResultType.Success, expectedResultMessage ) );

            var actualExceptionsCount = new ExceptionLogService( new RockContext() ).Queryable().Count();

            var actualJob = GetAddTestJob();

            Assert.That.AreEqual( expectedResultMessage, actualJob.LastStatusMessage );
            Assert.That.AreEqual( "Success", actualJob.LastStatus );

            Assert.That.AreEqual( expectedExceptionsCount, actualExceptionsCount );
        }

        public void RunJob( Dictionary<string, string> jobDataMapDictionary )
        {
            var job = GetAddTestJob();

            using ( var rockContext = new RockContext() )
            {
                var jobService = new ServiceJobService( rockContext );

                if ( job != null )
                {
                    // create a scheduler specific for the job
                    var scheduleConfig = new System.Collections.Specialized.NameValueCollection();
                    var runNowSchedulerName = ( "RunNow:" + job.Guid.ToString( "N" ) ).Truncate( 40 );
                    scheduleConfig.Add( StdSchedulerFactory.PropertySchedulerInstanceName, runNowSchedulerName );
                    var schedulerFactory = new StdSchedulerFactory( scheduleConfig );
                    var sched = new StdSchedulerFactory( scheduleConfig ).GetScheduler();
                    if ( sched.IsStarted )
                    {
                        // the job is currently running as a RunNow job
                        return;
                    }

                    // Check if another scheduler is running this job
                    try
                    {
                        var otherSchedulers = new Quartz.Impl.StdSchedulerFactory().AllSchedulers
                            .Where( s => s.SchedulerName != runNowSchedulerName );

                        foreach ( var scheduler in otherSchedulers )
                        {
                            if ( scheduler.GetCurrentlyExecutingJobs().Where( j => j.JobDetail.Description == job.Id.ToString() &&
                                j.JobDetail.ConcurrentExectionDisallowed ).Any() )
                            {
                                // A job with that Id is already running and ConcurrentExectionDisallowed is true
                                System.Diagnostics.Debug.WriteLine( RockDateTime.Now.ToString() + $" Scheduler '{scheduler.SchedulerName}' is already executing job Id '{job.Id}' (name: {job.Name})" );
                                return;
                            }
                        }
                    }
                    catch { }

                    // create the quartz job and trigger
                    IJobDetail jobDetail = jobService.BuildQuartzJob( job );

                    if ( jobDataMapDictionary != null )
                    {
                        // Force the <string, string> dictionary so that within Jobs, it is always okay to use
                        // JobDataMap.GetString(). This mimics Rock attributes always being stored as strings.
                        // If we allow non-strings, like integers, then JobDataMap.GetString() throws an exception.
                        jobDetail.JobDataMap.PutAll( jobDataMapDictionary.ToDictionary( kvp => kvp.Key, kvp => ( object ) kvp.Value ) );
                    }

                    var jobTrigger = TriggerBuilder.Create()
                        .WithIdentity( job.Guid.ToString(), job.Name )
                        .StartNow()
                        .Build();

                    // schedule the job
                    sched.ScheduleJob( jobDetail, jobTrigger );

                    // set up the listener to report back from the job when it completes
                    sched.ListenerManager.AddJobListener( new RockJobListener(), EverythingMatcher<JobKey>.AllJobs() );

                    // start the scheduler
                    sched.Start();

                    // Wait 10secs to give job chance to start
                    System.Threading.Tasks.Task.Delay( new TimeSpan( 0, 0, 10 ) ).Wait();

                    // stop the scheduler when done with job
                    sched.Shutdown( true );
                }
            }
        }

        private int testJobId;

        private Dictionary<string, string> GetJobDataMapDictionary( TestResultType resultType, string expectedExceptionMessage )
        {
            return new Dictionary<string, string> {
                { TestJobAttributeKey.ExecutionResult, resultType.ConvertToInt().ToString() },
                { TestJobAttributeKey.ExecutionMessage, expectedExceptionMessage }
            };
        }

        public ServiceJob GetAddTestJob()
        {
            var testJob = new ServiceJob
            {
                IsSystem = true,
                IsActive = true,
                Name = "Test Job",
                Description = "This job is used for testing RockJobListener",
                Class = "Rock.Tests.Integration.Jobs.RockJobListenerTestJob",
                CronExpression = "0 0 1 * * ?",
                NotificationStatus = JobNotificationStatus.All,
                Guid = "84AE12A7-968B-4D28-AB39-81D36D1F230E".AsGuid()
            };

            using ( var rockContext = new RockContext() )
            {
                var serviceJobService = new ServiceJobService( rockContext );

                var job = serviceJobService.Get( testJob.Guid );
                if ( job != null )
                {
                    testJobId = job.Id;
                    return job;
                }

                serviceJobService.Add( testJob );
                rockContext.SaveChanges();
            }

            testJobId = testJob.Id;
            return testJob;
        }

        public void RemoveTestJob()
        {
            if ( testJobId == 0 )
            {
                return;
            }

            using ( var rockContext = new RockContext() )
            {
                var serviceJobService = new ServiceJobService( rockContext );
                var testJob = serviceJobService.Get( testJobId );
                serviceJobService.Delete( testJob );
                rockContext.SaveChanges();
            }
        }
    }
}
