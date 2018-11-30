using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConcurrencyPrism.Infrastructure;
using ConcurrencyPrism.Infrastructure.AwaitableDelegateCommand;
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

    #region Function Tests

    [Test]
    public void CalculateMultiShouldReturnSquares()
    {
      _testee.Anzahl = 10;

      DelegateCommand command = _testee.CalculateMultiCommand;
      command?.Execute();

      _testee.ResultOutput.Should().Be(GetSquares(0, 9));
    }

    [Test]
    public async Task CalculateParallelShouldReturnSquares()
    {
      _testee.Anzahl = 10;

      AwaitableDelegateCommand command = _testee.CalculateMultiAsyncCommand;
      await command.ExecuteAsync(null);

      _testee.ResultOutput.Should().Be(GetSquares(0, 9));
    }

    [Test]
    public async Task CalculateParallelResultShouldBeVisible()
    {
      _testee.Anzahl = 3;

      AwaitableDelegateCommand command = _testee.CalculateMultiAsyncCommand;
      await command.ExecuteAsync(null);

      _testee.IsResultVisible.Should().Be(true);
    }

    [Test]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    [TestCase(6)]
    [TestCase(10)]
    public async Task CalculateParallelShouldReturnSquares(int anzahl)
    {
      _testee.Anzahl = anzahl;

      AwaitableDelegateCommand command = _testee.CalculateMultiAsyncCommand;
      await command.ExecuteAsync(null);

      _testee.ResultOutput.Should().Be(GetSquares(0, anzahl-1));
    }

    #endregion

    #region Cancellation Tests

    [Test]
    public async Task CancelCommandShouldCancelCalculation()
    {
      _testee.Anzahl = 3;

      await _testee.CalculateMultiAsyncCommand.ExecuteAsync(null);

      //Funktioniert nicht, da wegen dem await (anderer Thread) gar nie gecanceled wird
      _testee.CancelParallelCommand.Execute();
      
      _testee.ResultOutput.Should().Be("Calculation Cancelled");
    }

    #endregion

    #region Exception Tests


    #endregion

    #region Common

    private string GetSquares(int from, int to)
    {
      StringBuilder sb = new StringBuilder();
      for (int i = from; i <= to; i++)
      {
        if (sb.Length > 0) sb.Append(",");
        sb.Append((i * i).ToString());
      }

      return sb.ToString();
    }

    #endregion

  }
}
