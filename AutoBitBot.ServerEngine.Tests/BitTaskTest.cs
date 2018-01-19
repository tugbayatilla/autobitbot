// <copyright file="BitTaskTest.cs">Copyright ©  2018</copyright>
using System;
using System.Threading.Tasks;
using AutoBitBot.ServerEngine;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoBitBot.ServerEngine.Tests
{
    /// <summary>This class contains parameterized unit tests for BitTask</summary>
    [PexClass(typeof(BitTask))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class BitTaskTest
    {
        /// <summary>Test stub for CanExecute()</summary>
        [PexMethod]
        public bool CanExecuteTest([PexAssumeNotNull]BitTask target)
        {
            bool result = target.CanExecute();
            return result;
            // TODO: add assertions to method BitTaskTest.CanExecuteTest(BitTask)
        }

        /// <summary>Test stub for get_ExecuteAtEvery()</summary>
        [PexMethod]
        public int ExecuteAtEveryGetTest([PexAssumeNotNull]BitTask target)
        {
            int result = target.ExecuteAtEvery;
            return result;
            // TODO: add assertions to method BitTaskTest.ExecuteAtEveryGetTest(BitTask)
        }

        /// <summary>Test stub for Execute(Guid)</summary>
        [PexMethod]
        public Task<bool> ExecuteTest([PexAssumeNotNull]BitTask target, Guid executionId)
        {
            Task<bool> result = target.Execute(executionId);
            return result;
            // TODO: add assertions to method BitTaskTest.ExecuteTest(BitTask, Guid)
        }

        /// <summary>Test stub for get_Name()</summary>
        [PexMethod]
        public string NameGetTest([PexAssumeNotNull]BitTask target)
        {
            string result = target.Name;
            return result;
            // TODO: add assertions to method BitTaskTest.NameGetTest(BitTask)
        }

        /// <summary>Test stub for get_NextExecutionTime()</summary>
        [PexMethod]
        public DateTime NextExecutionTimeGetTest([PexAssumeNotNull]BitTask target)
        {
            DateTime result = target.NextExecutionTime;
            return result;
            // TODO: add assertions to method BitTaskTest.NextExecutionTimeGetTest(BitTask)
        }
    }
}
