using System.Text;
using System.Threading.Tasks;
using ConcurrencyPrism.Infrastructure.AwaitableDelegateCommand;
using ConcurrencyPrism.ViewModels;
using FluentAssertions;
using NUnit.Framework;
using Prism.Commands;


//Master

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

    #region CalculateMulti Tests

    [Test]
    public void CalculateMultiShouldReturnSquares()
    {
      _testee.Anzahl = 10;

      DelegateCommand command = _testee.CalculateMultiCommand;
      command?.Execute();

      _testee.ResultOutput.Should().Be(GetSquares(0, 9));
    }

    #endregion

    #region CalculateMultiAsync Tests

    [Test]
    public async Task CalculateMultiAsyncShouldReturnSquares()
    {
      _testee.Anzahl = 10;

      AwaitableDelegateCommand command = _testee.CalculateMultiAsyncCommand;
      await command.ExecuteAsync(null);

      _testee.ResultOutput.Should().Be(GetSquares(0, 9));
    }

    [Test]
    public async Task CalculateMultiAsyncResultShouldBeVisible()
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
    public async Task CalculateMultiAsyncShouldReturnSquares(int anzahl)
    {
      _testee.Anzahl = anzahl;

      AwaitableDelegateCommand command = _testee.CalculateMultiAsyncCommand;
      await command.ExecuteAsync(null);

      _testee.ResultOutput.Should().Be(GetSquares(0, anzahl-1));
    }

    #endregion

    #region CalculateParallelWhenAll Tests

    [Test]
    [TestCase(1)]
    [TestCase(3)]
    [TestCase(10)]
    public async Task TaskCalculateParallelWhenAllShouldReturnSquares(int anzahl)
    {
      _testee.Anzahl = anzahl;

      AwaitableDelegateCommand command = _testee.CalculateParallelWhenAllCommand;
      await command.ExecuteAsync(null);

      _testee.ResultOutput.Should().Be(GetSquares(0, anzahl - 1));
    }

    #endregion

    #region Cancellation Tests

    //[Test]
    public async Task CancelCommandShouldCancelCalculation()
    {
      _testee.Anzahl = 3;

      await _testee.CalculateMultiAsyncCommand.ExecuteAsync(null);

      //Funktioniert nicht, da wegen dem await (anderer Thread) gar nie gecanceled wird
      _testee.CancelParallelForEachCommand.Execute();
      
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
