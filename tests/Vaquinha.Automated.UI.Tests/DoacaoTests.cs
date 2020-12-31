using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using Vaquinha.Tests.Common.Fixtures;
using Xunit;

namespace Vaquinha.AutomatedUITests
{
	public class DoacaoTests : IDisposable, IClassFixture<DoacaoFixture>, 
                                               IClassFixture<EnderecoFixture>, 
                                               IClassFixture<CartaoCreditoFixture>
	{
		private DriverFactory _driverFactory = new DriverFactory();
		private IWebDriver _driver;

		private readonly DoacaoFixture _doacaoFixture;
		private readonly EnderecoFixture _enderecoFixture;
		private readonly CartaoCreditoFixture _cartaoCreditoFixture;

		public DoacaoTests(DoacaoFixture doacaoFixture, EnderecoFixture enderecoFixture, CartaoCreditoFixture cartaoCreditoFixture)
        {
            _doacaoFixture = doacaoFixture;
            _enderecoFixture = enderecoFixture;
            _cartaoCreditoFixture = cartaoCreditoFixture;
        }
		public void Dispose()
		{
			_driverFactory.Close();
		}

		[Fact]
		public void DoacaoUI_AcessoTelaHome()
		{
			// Arrange
			_driverFactory.NavigateToUrl("https://vaquinha.azurewebsites.net/");
			_driver = _driverFactory.GetWebDriver();

			// Act
			IWebElement webElement = null;
			webElement = _driver.FindElement(By.ClassName("vaquinha-logo"));

			// Assert
			webElement.Displayed.Should().BeTrue(because:"logo exibido");
		}

		[Fact]
		public void DoacaoUI_CriacaoDoacao()
		{
			//Arrange
			var doacao = _doacaoFixture.DoacaoValida();
            doacao.AdicionarEnderecoCobranca(_enderecoFixture.EnderecoValido());
            doacao.AdicionarFormaPagamento(_cartaoCreditoFixture.CartaoCreditoValido());
			_driverFactory.NavigateToUrl("https://vaquinha.azurewebsites.net/");
			_driver = _driverFactory.GetWebDriver();

			//Act
			IWebElement webElement = null;
			webElement = _driver.FindElement(By.ClassName("btn-yellow"));
			webElement.Click();

			//Assert
			_driver.Url.Should().Contain("/Doacoes/Create");
		}

		[Fact]
		public void DoacaoUI_VerificaDoacaoValida()
		{
			//Arrange
			var doacao = _doacaoFixture.DoacaoValida();
			_driverFactory.NavigateToUrl("https://vaquinha.azurewebsites.net/Doacoes/Create");
			_driver = _driverFactory.GetWebDriver();

			//Act
			IWebElement campoValor = _driver.FindElement(By.Name("Valor"));
			campoValor.SendKeys("10");

			IWebElement campoNome = _driver.FindElement(By.Id("DadosPessoais_Nome"));
			System.Threading.Thread.Sleep(1000);
			campoNome.SendKeys("Marcos Silva");

			IWebElement campoEndereco = _driver.FindElement(By.Id("EnderecoCobranca_TextoEndereco"));
			campoEndereco.SendKeys("Avenida Los Angeles");

			IWebElement campoNumero = _driver.FindElement(By.Id("EnderecoCobranca_Numero"));
			campoNumero.SendKeys("253");

			IWebElement campoEmail = _driver.FindElement(By.Id("DadosPessoais_Email"));
			campoEmail.SendKeys("marcos.silva@gmail.com");

			IWebElement campoCidade = _driver.FindElement(By.Id("EnderecoCobranca_Cidade"));
			campoCidade.SendKeys("Jacareí");

			IWebElement campoEstado = _driver.FindElement(By.Id("estado"));
			campoEstado.SendKeys("São Paulo");

			IWebElement campoCep = _driver.FindElement(By.Id("cep"));
			campoCep.SendKeys("12333-789");

			IWebElement campoComplemento = _driver.FindElement(By.Id("EnderecoCobranca_Complemento"));
			campoComplemento.SendKeys("Casa");

			IWebElement campoTelefone = _driver.FindElement(By.Id("telefone"));
			campoTelefone.SendKeys("(12) 98811-4545");

			IWebElement campoTitular = _driver.FindElement(By.Id("FormaPagamento_NomeTitular"));
			campoTitular.SendKeys("");

			IWebElement campoCardNumber = _driver.FindElement(By.Id("cardNumber"));
			campoCardNumber.SendKeys("5254162342864154");

			IWebElement campoValidade = _driver.FindElement(By.Id("validade"));
			campoValidade.SendKeys("07/2021");

			IWebElement campoCvv = _driver.FindElement(By.Id("cvv"));
			campoCvv.SendKeys("914");

			IWebElement webElement = null;
			webElement = _driver.FindElement(By.ClassName("btn-yellow"));
			webElement.Click();

			//Assert
			_driver.Url.Should().Contain("vaquinha.azurewebsites.net");

		}

	}
}