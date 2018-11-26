using System;
using System.Threading.Tasks;
using ConcurrencyPrism.ViewModels;
using FluentAssertions;
using NUnit.Framework;
using Prism.Commands;

namespace UnitTestProject1
{
  [TestFixture]
  public class ConcurrencyViewModelTest
  {

    private ConcurrencyViewModel _testee;

    [SetUp]
    public void Init()
    {
      _testee = new ConcurrencyViewModel();
    }

    #region Basic Tests

    [Test]
    public void CalculateMultiShouldReturnSquares()
    {
      _testee.Anzahl = 10;

      DelegateCommand command = _testee.CalculateMultiCommand as DelegateCommand;
      command?.Execute();

      _testee.ResultOutput.Should().Be("1,4,9,16,25,36,49,64,81");
    }

    //public Task CalculateParallelShouldReturnSquares()
    //{
    //  _testee.Anzahl = 10;

    //  //DelegateCommand command = DelegateCommand.FromAsyncHandler(async () => await MyMethod());

    //  //DelegateCommand command = (DelegateCommand)_testee.CalculateParallelCommand;
    //  //DelegateCommand = new DelegateCommand<string>(async (x) => await (DelegateCommand)_testee.CalculateParallelCommand());

    //  //DelegateCommand command = _testee.CalculateParallelCommand as DelegateCommand;
    //  //await command?.Execute();

    //  _testee.ResultOutput.Should().Be("1,4,9,16,25,36,49,64,81");
    //}

    [Test]
    public void CalculateParallelProgressBarShouldBeHidden()
    {
      _testee.Anzahl = 3;

      DelegateCommand command = _testee.CalculateParallelCommand as DelegateCommand;
      command?.Execute();

      _testee.IsProgressBarVisible.Should().Be(false);
    }

    [Test]
    public void CalculateParallelResultShouldBeVisible()
    {
      _testee.Anzahl = 3;

      DelegateCommand command = _testee.CalculateParallelCommand as DelegateCommand;
      command?.Execute();

      _testee.IsResultVisible.Should().Be(true);
    }

    #endregion

    #region Parameter Tests

    //[Test]
    //[TestCase(3)]
    //[TestCase(10)]
    //[TestCase(100)]
    //public async Task GetValueShouldReturnValueAsync(int value)
    //{
    //  _testee.Anzahl = 10;

    //  int result = await _testee.CalculateParallel();

    //  result.Should().Be(value);
    //}

    #endregion

    #region Exception Tests

    //[Test]
    //public void GetExceptionShouldThrowException()
    //{
    //  Action act = () => _testee.Exception();

    //  act.Should().Throw<Exception>();
    //}

    //[Test]
    //public void GetExceptionShouldThrowExceptionAsync()
    //{
    //  Func<Task> func = async () => await _testee.ExceptionAsync();
    //  func.Should().Throw<Exception>();
    //}

    #endregion

  }
}
