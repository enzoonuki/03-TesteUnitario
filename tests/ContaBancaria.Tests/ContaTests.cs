using Xunit;
using ContaBancaria;

namespace ContaBancaria.Tests;

/// <summary>
/// Testes unitários para a classe Conta.
/// 
/// PARTE 1 — Testes de exemplo (Construtor) já estão prontos.
///           Observe o padrão AAA e o uso de [Fact] e [Theory].
///
/// PARTE 2 — Você deve escrever os testes para os demais métodos
///           seguindo rigorosamente o ciclo TDD: Red → Green → Refactor.
///
/// Para cada método da classe Conta, crie testes que cubram:
///   ✅ O cenário de sucesso (caminho feliz)
///   ❌ Cada regra de validação (cenários de exceção)
///   🔄 Casos de borda (valores limites)
/// </summary>
public class ContaTests
{
    // =======================================================
    //  PARTE 1 — EXEMPLO GUIADO: Testes do Construtor
    //  Observe o padrão Arrange-Act-Assert (AAA)
    // =======================================================

    [Fact]
    public void Construtor_DadosValidos_CriaContaCorretamente()
    {
        // Arrange & Act
        var conta = new Conta("Maria", 100);

        // Assert
        Assert.Equal("Maria", conta.Titular);
        Assert.Equal(100, conta.Saldo);
        Assert.True(conta.Ativa);
    }

    [Fact]
    public void Construtor_SemSaldoInicial_CriaContaComSaldoZero()
    {
        // Arrange & Act
        var conta = new Conta("João");

        // Assert
        Assert.Equal("João", conta.Titular);
        Assert.Equal(0, conta.Saldo);
        Assert.True(conta.Ativa);
    }

    [Fact]
    public void Construtor_TitularNulo_LancaArgumentException()
    {
        // Assert — verifica que a exceção é lançada
        Assert.Throws<ArgumentException>(() => new Conta(null!));
    }

    [Fact]
    public void Construtor_TitularVazio_LancaArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Conta(""));
    }

    [Fact]
    public void Construtor_SaldoNegativo_LancaArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Conta("Maria", -50));
    }

    [Theory]
    [InlineData("Ana", 0)]
    [InlineData("Carlos", 1000)]
    [InlineData("Beatriz", 0.01)]
    public void Construtor_VariosValoresValidos_CriaContaCorretamente(string titular, decimal saldo)
    {
        // Act
        var conta = new Conta(titular, saldo);

        // Assert
        Assert.Equal(titular, conta.Titular);
        Assert.Equal(saldo, conta.Saldo);
        Assert.True(conta.Ativa);
    }

    // =======================================================
    //  PARTE 2 — ESCREVA OS TESTES ABAIXO (TDD)
    //  Lembre-se: escreva o teste PRIMEIRO, veja FALHAR (Red),
    //  depois implemente o código para PASSAR (Green),
    //  e por fim faça Refactor se necessário.
    // =======================================================

    // =======================================================
    //  Testes para Depositar
    //  Sugestão de testes:
    //    - Depósito com valor válido atualiza o saldo
    //    - Depósito com valor zero lança ArgumentException
    //    - Depósito com valor negativo lança ArgumentException
    //    - Depósito em conta inativa lança InvalidOperationException
    // =======================================================
    [Fact]
    public void Depositar_ValorValido_AtualizaSaldo()
    {
        // Arrange
        var conta = new Conta("Maria", 100);

        // Act
        conta.Depositar(50);

        // Assert
        Assert.Equal(150, conta.Saldo);
    }

    [Fact]
    public void Depositar_ValorZero_LancaArgumentException()
    {
        // Arrange
        var conta = new Conta("Maria", 100);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => conta.Depositar(0));
    }

    [Fact]
    public void Depositar_ValorNegativo_LancaArgumentException()
    {
        // Arrange
        var conta = new Conta("Maria", 100);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => conta.Depositar(-50));
    }

    [Fact]
    public void Depositar_ContaInativa_LancaInvalidOperationException()
    {
        // Arrange
        var conta = new Conta("Maria", 0);
        conta.Encerrar();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => conta.Depositar(50));
    }

    // =======================================================
    //  Testes para Sacar
    //  Sugestão de testes:
    //    - Saque com valor válido atualiza o saldo
    //    - Saque com valor maior que saldo lança InvalidOperationException
    //    - Saque com valor zero lança ArgumentException
    //    - Saque com valor negativo lança ArgumentException
    //    - Saque em conta inativa lança InvalidOperationException
    // =======================================================
    [Fact]
    public void Sacar_ValorValido_AtualizaSaldo()
    {
        // Arrange
        var conta = new Conta("Maria", 100);

        // Act
        conta.Sacar(50);

        // Assert
        Assert.Equal(50, conta.Saldo);
    }

    [Fact]
    public void Sacar_ValorMaiorQueSaldo_LancaInvalidOperationException()
    {
        // Arrange
        var conta = new Conta("Maria", 100);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => conta.Sacar(150));
    }

    [Fact]
    public void Sacar_ValorZero_LancaArgumentException()
    {
        // Arrange
        var conta = new Conta("Maria", 100);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => conta.Sacar(0));
    }

    [Fact]
    public void Sacar_ValorNegativo_LancaArgumentException()
    {
        // Arrange
        var conta = new Conta("Maria", 100);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => conta.Sacar(-50));
    }

    [Fact]
    public void Sacar_ContaInativa_LancaInvalidOperationException()
    {
        // Arrange
        var conta = new Conta("Maria", 0);
        conta.Encerrar();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => conta.Sacar(50));
    }

    // =======================================================
    //  Testes para Transferir
    //  Sugestão de testes:
    //    - Transferência válida atualiza saldo de ambas as contas
    //    - Transferência com saldo insuficiente lança exceção
    //    - Transferência com valor zero/negativo lança exceção
    //    - Transferência com conta origem inativa lança exceção
    //    - Transferência com conta destino inativa lança exceção
    // =======================================================

    [Fact]
    public void Transferir_ValorValido_AtualizaSaldoAmbasContas()
    {
        // Arrange
        var contaOrigem = new Conta("Maria", 100);
        var contaDestino = new Conta("João", 50);

        // Act
        contaOrigem.Transferir(contaDestino, 30);
        // Assert
        Assert.Equal(70, contaOrigem.Saldo);
        Assert.Equal(80, contaDestino.Saldo);
    }

    [Fact]
    public void Transferir_SaldoInsuficiente_LancaInvalidOperationException()
    {
        // Arrange
        var contaOrigem = new Conta("Maria", 100);
        var contaDestino = new Conta("João", 50);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => contaOrigem.Transferir(contaDestino,
            150));
    }

    [Fact]
    public void Transferir_ValorZeroOuNegativo_LancaArgumentException()
    {
        // Arrange
        var contaOrigem = new Conta("Maria", 100);
        var contaDestino = new Conta("João", 50);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => contaOrigem.Transferir(contaDestino, 0
        ));
        Assert.Throws<ArgumentException>(() => contaOrigem.Transferir(contaDestino, -50
        ));
    }

    [Fact]
    public void Transferir_ContaOrigemInativa_LancaInvalidOperationException()
    {
        // Arrange
        var contaOrigem = new Conta("Maria", 0);
        var contaDestino = new Conta("João", 100);
        contaOrigem.Encerrar();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => contaOrigem.Transferir(contaDestino,
            30));
    }

    [Fact]
    public void Transferir_ContaDestinoInativa_LancaInvalidOperationException()
    {
        // Arrange
        var contaOrigem = new Conta("Maria", 100);
        var contaDestino = new Conta("João", 0);
        contaDestino.Encerrar();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => contaOrigem.Transferir(contaDestino,
            30));
    }

    // =======================================================
    //  Testes para Encerrar
    //  Sugestão de testes:
    //    - Encerrar conta com saldo zero funciona
    //    - Encerrar conta com saldo lança InvalidOperationException
    //    - Encerrar conta já inativa lança InvalidOperationException
    //    - Conta encerrada tem Ativa == false
    // =======================================================

    [Fact]
    public void Encerrar_ContaComSaldoZero_EncerraComSucesso()
    {
        // Arrange
        var conta = new Conta("Maria", 0);

        // Act
        conta.Encerrar();

        // Assert
        Assert.False(conta.Ativa);
    }

    [Fact]
    public void Encerrar_ContaComSaldo_LancaInvalidOperationException()
    {
        // Arrange
        var conta = new Conta("Maria", 100);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => conta.Encerrar());
    }

    [Fact]
    public void Encerrar_ContaJaInativa_LancaInvalidOperationException()
    {
        // Arrange
        var conta = new Conta("Maria", 0);
        conta.Encerrar();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => conta.Encerrar());
    }

    [Fact]
    public void Encerrar_ContaAtiva_AtivaFicaFalse()
    {
        // Arrange
        var conta = new Conta("Maria", 0);

        // Act
        conta.Encerrar();

        // Assert
        Assert.False(conta.Ativa);
    }
}
