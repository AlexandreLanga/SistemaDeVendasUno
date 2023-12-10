using Microsoft.VisualBasic;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using System.Text;
using SistemaDeVendas.Classes;
using System.Linq.Expressions;
using System.Globalization;

internal class Program
{
    private static void Main(string[] args)
    {
        Login();
    }
    #region Cadastros
    public static void MenuCadastros(string usuario)
    {
        Console.Clear();
        Console.WriteLine("1 - Usuário");
        Console.WriteLine("2 - Produto");
        Console.WriteLine("3 - Relatório de usuários cadastrados");
        Console.WriteLine("4 - Voltar");

        int opcaoCadastro = int.Parse(Console.ReadLine());

        switch (opcaoCadastro)
        {
            case 1:
                if (usuario == "admin")
                {
                    CadastroUsuario(usuario);
                }
                else
                {
                    Console.WriteLine("Somente o administrador do sistema pode cadastrar um usuário novo!");
                    Console.ReadKey();
                    MenuPrincipal(usuario);
                }
                break;
            case 2:
                CadastroProduto(usuario);
                break;
            case 3:
                RelatorioUsuarios(usuario);
                break;
            case 4:
                MenuPrincipal(usuario);
                break;
        }
    }
    public static void CadastroProduto(string usuario)
    {
        Console.Clear();
        Produto produto = new Produto();

        Console.WriteLine("Insira o código do produto:");
        produto.Id = int.Parse(Console.ReadLine());

        Console.WriteLine("Insira o nome do produto:");
        produto.Nome = Console.ReadLine();

        Console.WriteLine("Insira o preço do produto:");
        produto.Preco = float.Parse(Console.ReadLine());

        Console.WriteLine("Insira a descrição do produto:");
        produto.Descricao = Console.ReadLine();

        string diretorio = @"C:\SistemaDeVendas\Produtos";
        string arquivo = "Produtos.txt";

        string caminho = Path.Combine(diretorio, arquivo);

        try
        {
            if (!Directory.Exists(diretorio))
            {
                Directory.CreateDirectory(diretorio);
                Console.WriteLine("Diretório criado com sucesso!");
            }

            if (!File.Exists(caminho))
            {
                using (File.Create(caminho)) { }

                Console.WriteLine("Arquivo criado com sucesso!");
            }

            string dados = $"{produto.Id}\n{produto.Nome}\n{produto.Preco}\n{produto.Descricao}\n";
            File.AppendAllText(caminho, dados);

            Console.WriteLine("Seu produto foi cadastrado com sucesso! Verifique!\n");
            GravarLog(usuario, $"Novo produto {produto.Nome} cadastrado");
            Console.WriteLine("Você será encaminhado de volta para o menu principal");
            Console.ReadKey();
            MenuPrincipal(usuario);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocorreu um erro: {ex.Message}");
            Console.WriteLine("Apesar do erro, você será encaminhado para o menu principal, verifique o possível erro! Pressione qualquer tecla para continuar");
            Console.ReadKey();
            MenuPrincipal(usuario);
        }
    }
    public static void CadastroUsuario(string usuarioLogado)
    {
        Console.Clear();
        Usuario usuario = new Usuario();

        Console.WriteLine("Insira o login do usuário:");
        usuario.Login = Console.ReadLine();

        Console.WriteLine("Insira a senha do usuário:");
        usuario.Senha = Console.ReadLine();

        string diretorio = @"C:\SistemaDeVendas\Usuarios";
        string arquivo = "Usuarios.txt";

        string caminho = Path.Combine(diretorio, arquivo);

        try
        {
            if (!Directory.Exists(diretorio))
            {
                Directory.CreateDirectory(diretorio);
                Console.WriteLine("Diretório criado com sucesso!");
            }

            if (!File.Exists(caminho))
            {
                using (File.Create(caminho)) { }

                Console.WriteLine("Arquivo criado com sucesso!");
            }

            string dados = $"{usuario.Login}\n{usuario.Senha}\n";
            File.AppendAllText(caminho, dados);

            Console.WriteLine("Novo usuário cadastrado com sucesso! Verifique!");
            GravarLog(usuarioLogado, $"Novo usuário {usuario.Login} cadastrado");
            Console.WriteLine("Você será encaminhado para o menu principal, pressione qualquer tecla para continuar");
            Console.ReadKey();
            MenuPrincipal(usuarioLogado);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocorreu um erro: {ex.Message}");
            Console.WriteLine("Apesar do erro, você será encaminhado para o menu principal, verifique o possível erro! Pressione qualquer tecla para continuar");
            Console.ReadKey();
            MenuPrincipal(usuarioLogado);
        }
    }
    public static void RelatorioUsuarios(string usuarioLogado)
    {
        Console.Clear();
        //Lê do diretório
        string diretorio = @"C:\SistemaDeVendas\Usuarios";
        string arquivo = "Usuarios.txt";
        int contador = 1, contaVetor = 0;
        string[] usuariosLidos = new string[100];
        string caminho = Path.Combine(diretorio, arquivo);
        string[] usuarios = File.ReadAllLines(caminho);

        try
        {
            if (!Directory.Exists(diretorio))
            {
                Directory.CreateDirectory(diretorio);
                Console.WriteLine("Diretório criado com sucesso!");
            }

            if (!File.Exists(caminho))
            {
                using (File.Create(caminho)) { }

                Console.WriteLine("Arquivo criado com sucesso!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocorreu um erro: {ex}, apesar do erro você será encaminhado ao menu principal!");
            Console.WriteLine("Pressione qualquer tecla para continuar");
            Console.ReadKey();
            MenuPrincipal(usuarioLogado);
        }

        var checaArquivo = new FileInfo(caminho);

        //Verifica se o arquivo tem dados, se não tiver, volta ao menu principal
        if (checaArquivo.Length == 0)
        {
            Console.WriteLine($"Não existem usuários cadastrados! Você será encaminhado ao menu principal!");
            Console.ReadKey();
            MenuPrincipal(usuarioLogado);
        }
        else
        {
            //Lê os usuários e os armazena em um vetor para depois ordená-los
            foreach (string usuario in usuarios)
            {
                if (contador % 2 != 0)
                {
                    usuariosLidos[contaVetor] = usuario;
                    contaVetor++;
                }
                contador++;
            }

            // Ordena o vetor
            Array.Sort(usuariosLidos);
            //Reseta o contador para reaproveita-lo
            contador = 0;

            foreach (string usuario in usuariosLidos)
            {
                //Se a posição do que foi lida estiver vazia, conta o espaço disponível para o relatório (tamanho do vetor)
                if (usuario == null)
                {
                    contador++;
                }
                else
                {
                    //Se não, printa os usuários já ordenados
                    Console.WriteLine(usuario);
                }
            }
            Console.WriteLine($"Você ainda tem {contador} espaços de armazenamento para gerar esse relatório\n");
            Console.WriteLine("Pressione qualquer tecla para voltar ao menu principal!");
            Console.ReadKey();
            MenuPrincipal(usuarioLogado);
        }
    }
    #endregion

    #region Vendas
    public static void MenuVendas(string usuario)
    {
        //Apresenta as opções disponíveis das vendas
        Console.Clear();
        Console.WriteLine("1 - Vender");
        Console.WriteLine("2 - Relatório de vendas");
        Console.WriteLine("3 - Voltar");

        int opcaoVendas = int.Parse(Console.ReadLine());

        switch (opcaoVendas)
        {
            case 1:
                Vender(usuario);
                break;

            case 2:
                if (usuario == "admin")
                {
                    RelatorioVendas(usuario);
                }
                else
                {
                    Console.WriteLine("Somente o administrador do sistema pode ter acesso a aba de relatórios!");
                    Console.ReadKey();
                    MenuPrincipal(usuario);
                }

                break;
            case 3:
                MenuPrincipal(usuario);
                break;
        }
    }
    public static void Vender(string usuario)
    {
        //Aqui o código da uma elevada no nível, nessa função é lido os produtos dispóveis que foram cadastrados, que são armazenados em um vetor para serem passados
        //Para a venda efetiva do produto

        string diretorio = @"C:\SistemaDeVendas\Produtos";
        string arquivo = "Produtos.txt";

        string caminho = Path.Combine(diretorio, arquivo);

        string[] produtos = File.ReadAllLines(caminho);

        var checaArquivo = new FileInfo(caminho);
        //Checa se tem produtos para vender, se não tiver volta ao menu principal
        if (checaArquivo.Length == 0)
        {
            Console.WriteLine("Não existem produtos cadastrados para venda! Você será encaminhado ao menu principal!");
            Console.ReadKey();
            MenuPrincipal(usuario);
        }
        else
        {
            //Declara as variáveis que serão necessárias para ler os produtos e armazená-los para usar-los na venda
            int contador = 1, contCod = 1, contNome = 2, contPre = 3, contDesc = 4;
            string codigo = "", nome = "", preco = "", descricao = "";
            string[] produtosLidos = new string[9999];
            Console.Clear();
            Console.WriteLine("Selecione o código do produto:\n");
            //Aqui é necessário entender o cadastro dos produtos, como eles são armazenados de 4 em 4 linhas, fica uma linha para cada dado
            //Linha 1 - código do produto
            //Linha 2 - nome do produto
            //Linha 3 - preço do produto
            //Linha 4 - descrição do produto
            //Para cada linha, o contador armazena uma informação em uma variavel, que na descrição, pega todos os anteriores e guarda num vetor para mostrar os produtos ao usuário
            foreach (string produto in produtos)
            {
                if (contador == 1)
                {
                    codigo = produto;

                }
                else
                {
                    if (contador == 2)
                    {
                        nome = produto;
                    }
                    else
                    {
                        if (contador == 3)
                        {
                            preco = produto;
                        }
                        else
                        {
                            if (contador == 4)
                            {
                                descricao = produto;
                                //Segue o padrão 4 em 4 para ficar linear, cada variavél tem um contador próprio para diferenciar a posição dos seus vetores
                                //Foram incializadas com valores diferentes para ficar mais óbvio
                                Console.WriteLine($"Código: {codigo}\n{nome}\nPreço = R${preco}\nDescrição: {descricao}\n");

                                produtosLidos[contCod] = codigo;
                                produtosLidos[contNome] = nome;
                                produtosLidos[contPre] = preco;
                                produtosLidos[contDesc] = descricao;

                                contCod += 4;
                                contNome += 4;
                                contPre += 4;
                                contDesc += 4;

                                contador = 0;
                            }
                        }
                    }
                }
                contador++;
            }
            //Depois de mostrar todos os produtos cadastrados, oferece a opção do usuário de escolher o produto pelo código
            string opcaoProduto = (Console.ReadLine());
            int posicaoVetorCod = 1;

            //Pode percorrer todo o espaço do vetor que foi declarado para o mesmo, quando achar o código do produto, quebra o laço e efetua a venda
            for (int i = 0; i < 9999; i++)
            {
                if (opcaoProduto == produtosLidos[posicaoVetorCod])
                {
                    //Passa o código, nome, preço e descrição para a venda dos produtos
                    //Mais uma vez, como é de 4 em 4, os dados sempre seguem esse padrão
                    EfetuarVenda(produtosLidos[posicaoVetorCod], produtosLidos[posicaoVetorCod + 1], produtosLidos[posicaoVetorCod + 2], produtosLidos[posicaoVetorCod + 3], usuario);
                    break;
                }
                else
                {
                    //Pula até o próximo código enquanto não achar o código
                    posicaoVetorCod += 4;
                }
                i++;
            }
            //Aqui poderia ter uma validação de caso o usuário digitar errado, porém pelo tempo de apresentação não foi feito, fica pra próxima
        }
    }
    public static void RelatorioVendas(string usuario)
    {
        //Apresenta as opções do menu de vendas e encaminha as suas respectivas funções
        DateOnly dataHoje = DateOnly.FromDateTime(DateTime.Now);
        Console.Clear();
        Console.WriteLine("Escolha o tipo de relatório a ser gerado:");
        Console.WriteLine("1 - Relatório geral de vendas");
        Console.WriteLine($"2 - Relatório de vendas do dia de hoje: {dataHoje}");
        Console.WriteLine("3 - Voltar");

        int opcaoRelatorio = int.Parse(Console.ReadLine());

        switch (opcaoRelatorio)
        {
            case 1:
                RelatorioGeral(usuario);
                break;
            case 2:
                RelatorioDiario(usuario);
                break;
            case 3:
                MenuPrincipal(usuario);
                break;
        }
    }
    public static void RelatorioGeral(string usuario)
    {
        //Lê o arquivo de vendas sem filtrar nada, só imprime tudo o que foi gravado nas vendas de todos os períodos, depois volta ao menu principal
        Console.Clear();
        string diretorio = @"C:\SistemaDeVendas\Vendas";
        string arquivo = "Vendas.txt";
        string caminho = Path.Combine(diretorio, arquivo);
        string[] vendas = File.ReadAllLines(caminho);
        GravarLog(usuario, $"{usuario} acessou o relatório geral de vendas");

        foreach (string venda in vendas)
        {
            Console.WriteLine(venda);
        }

        Console.WriteLine("\nPressione qualquer botão para voltar ao menu principal");
        Console.ReadKey();
        MenuPrincipal(usuario);
    }
    public static void RelatorioDiario(string usuario)
    {
        Console.Clear();
        //Declara as variáveis que serão usadas para as validações
        int contador = 1;
        bool imprimir = false;
        string converteData = "";
        string diretorio = @"C:\SistemaDeVendas\VendasDoDia";
        string arquivo = "VendasDoDia.txt";
        string caminho = Path.Combine(diretorio, arquivo);
        DateOnly dataHoje = DateOnly.FromDateTime(DateTime.Now);
        string textoProcurado = "Preço:";

        //Se não existir o diretório das vendas do dia, vai criar ele
        try
        {
            if (!Directory.Exists(diretorio))
            {
                Directory.CreateDirectory(diretorio);
                Console.WriteLine("Diretório criado com sucesso!");
            }

            if (!File.Exists(caminho))
            {
                using (File.Create(caminho)) { }

                Console.WriteLine("Arquivo criado com sucesso!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocorreu um erro: {ex}");
            Console.WriteLine("Apesar do erro, você será encaminhado ao menu inicial! Verifique o possível erro");
            MenuPrincipal(usuario);
        }

        string[] vendas = File.ReadAllLines(caminho);
        GravarLog(usuario, $"{usuario} acessou o relatório diário de vendas");

        var checaArquivo = new FileInfo(caminho);

        //Verifica se o arquivo tem dados, se não tiver, volta ao menu
        if (checaArquivo.Length == 0)
        {
            Console.WriteLine($"Não foram vendidos produtos no dia: {dataHoje}! Realize uma venda!");
            Console.ReadKey();
            MenuPrincipal(usuario);
        }
        else
        {
            foreach (string venda in vendas)
            {
                //Aqui a parada é medonha
                //Vai ler a primeira data do registro, cortando a string, para converte-lá em DateOnly e validar se o registro é de outro dia para atualizar o registro
                if (contador == 4)
                {
                    converteData = venda;
                    // Encontra a posição inicial da data tendo como referência o :, e pula os 2 espaços no arquivo para começar pelo primeiro caractere da data
                    int indiceInicio = converteData.IndexOf(':') + 2;

                    // Obtém a data cheia, que são os 10 caracteres "10/12/2023" por exemplo
                    string dataString = converteData.Substring(indiceInicio, 10);

                    // Converte a string da data para um objeto do tipo DateTime.
                    if (DateOnly.TryParse(dataString, out DateOnly data))
                    {
                        //Se a data for diferente do dia atual (virou o dia), vai excluir o arquivo, forçando a geração de um novo, e como tem a validação se ele está vazio
                        //Só vai gerar o relatório se o usuário cadastrar uma venda nesse novo dia
                        if (data != dataHoje)
                        {
                            File.Delete(caminho);
                            Console.WriteLine($"Ainda não foram realizadas vendas no dia: {dataHoje}! Realize uma venda!");
                            Console.ReadKey();
                            MenuPrincipal(usuario);
                            break;
                        }
                        else
                        {
                            imprimir = true;
                            break;
                        }
                    }
                }
                contador++;
            }
            //Se a primeira venda foi efetuada, e o dia é o mesmo, vai gerar o relatório nesse foreach
            //Zera o contador para reutilizá-lo
            contador = 1;
            bool primeriraRodada = true;
            float ValorDiario = 0, somaValorDiario = 0;
            if (imprimir == true)
            {
                foreach (string venda in vendas)
                {
                    Console.WriteLine(venda);
                    //O primeiro valor sempre vai ser na linha 7, e os outros na linha 9, é feito um controle para diferenciar as linhas
                    if((contador == 7 && primeriraRodada == true) || contador  == 9)
                    {
                        ValorDiario = ExtrairValorNumericoComoFloat(venda);
                        somaValorDiario += ValorDiario;
                        primeriraRodada = false;
                        contador = 1;
                    }
                    contador++;
                }
                Console.WriteLine($"\nSoma total das vendas no dia {dataHoje}: R${somaValorDiario}");
                Console.WriteLine("\nPressione qualquer botão para voltar ao menu principal");
                Console.ReadKey();
                MenuPrincipal(usuario);
            }
        }
    }
    public static void EfetuarVenda(string codigo, string nome, string preco, string descricao, string usuario)
    {
        //Nessa função, lê os dados informados para a venda e grava a venda realizada em arquivo
        Console.Clear();
        Venda venda = new Venda();

        Console.WriteLine("Informe o nome do cliente:");
        venda.Cliente = Console.ReadLine();

        Console.WriteLine("Escolha uma opção de pagamento:");

        foreach (Venda.Pagamento opcao in Enum.GetValues(typeof(Venda.Pagamento)))
        {
            Console.WriteLine($"{(int)opcao}. {opcao}");
        }

        Console.Write("Digite o número da opção desejada: ");

        string pagamento = Console.ReadLine();

        if (Enum.TryParse(pagamento, out Venda.Pagamento escolha))
        {
            Console.WriteLine($"Você escolheu: {escolha}");
        }
        else
        {
            Console.WriteLine("Opção inválida! Tente novamente!");
            SelecionaPagamento();
        }

        venda.Data = DateTime.Now;

        Console.WriteLine("Deseja realizar alguma nota sobre a compra?");
        Console.WriteLine("1 - Sim");
        Console.WriteLine("2 - Não");
        int anotar = int.Parse(Console.ReadLine());

        if (anotar == 1)
        {
            Console.WriteLine("Digite sua anotação:");
            venda.Anotacao = Console.ReadLine();
        }
        else
        {
            venda.Anotacao = "Sem anotações";
        }

        Console.WriteLine("Deseja confirmar a venda?");
        Console.WriteLine("1 - Confirmar");
        Console.WriteLine("2 - Cancelar");

        int confCompra = int.Parse(Console.ReadLine());
        //Se confirmar a compra, grava os dados no arquivo de vendas diário e geral
        if (confCompra == 1)
        {
            string diretorio = @"C:\SistemaDeVendas\Vendas";
            string arquivo = "Vendas.txt";
            string diretorioDiario = @"C:\SistemaDeVendas\VendasDoDia";
            string arquivoDiario = "VendasDoDia.txt";

            string caminho = Path.Combine(diretorio, arquivo);
            string caminhoDiario = Path.Combine(diretorioDiario, arquivoDiario);

            //Grava a venda no arquivo de vendas diárias
            try
            {
                if (!Directory.Exists(diretorioDiario))
                {
                    Directory.CreateDirectory(diretorioDiario);
                    Console.WriteLine("Diretório criado com sucesso!");
                }

                if (!File.Exists(caminhoDiario))
                {
                    using (File.Create(caminhoDiario)) { }

                    Console.WriteLine("Arquivo criado com sucesso!");
                }

                string dados = $"\nCliente: {venda.Cliente}\nPagamento:{escolha}\nData: {venda.Data}\nAnotações: {venda.Anotacao}\n" +
                    $"Produto: {codigo} - {nome}\nPreço: {preco}\nDescrição:{descricao}\nVenda feita por {usuario}";

                File.AppendAllText(caminhoDiario, dados);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro: {ex.Message}");
                Console.WriteLine("Apesar do erro, você será encaminhado para o menu principal, verifique o possível erro! Pressione qualquer tecla para continuar");
                Console.ReadKey();
                MenuPrincipal(usuario);
            }
            //Grava a venda no arquivo de vendas gerais
            try
            {
                if (!Directory.Exists(diretorio))
                {
                    Directory.CreateDirectory(diretorio);
                    Console.WriteLine("Diretório criado com sucesso!");
                }

                if (!File.Exists(caminho))
                {
                    using (File.Create(caminho)) { }

                    Console.WriteLine("Arquivo criado com sucesso!");
                }

                string dados = $"\nCliente: {venda.Cliente}\nPagamento:{escolha}\nData: {venda.Data}\nAnotações: {venda.Anotacao}\n" +
                    $"Produto: {codigo} - {nome}\nPreço: {preco}\nDescrição:{descricao}\nVenda feita por {usuario}";

                File.AppendAllText(caminho, dados);

                Console.WriteLine("Compra registrada! Verifique!");
                Console.WriteLine("Você será encaminhado para o menu principal, pressione qualquer tecla para continuar");
                Console.ReadKey();
                MenuPrincipal(usuario);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro: {ex.Message}");
                Console.WriteLine("Apesar do erro, você será encaminhado para o menu principal, verifique o possível erro! Pressione qualquer tecla para continuar");
                Console.ReadKey();
                MenuPrincipal(usuario);
            }
        }
        else //Se não confirmar, volta ao menu principal
        {
            Console.WriteLine("Compra cancelada! Você será encaminhado de volta ao menu principal");
            Console.ReadKey();
            MenuPrincipal(usuario);
        }
    }
    public static void SelecionaPagamento()
    {
        //Se a opção de pagamento for inválida, repete por recursividade até ser válida
        Console.WriteLine("Escolha uma opção de pagamento:");

        foreach (Venda.Pagamento opcao in Enum.GetValues(typeof(Venda.Pagamento)))
        {
            Console.WriteLine($"{(int)opcao}. {opcao}");
        }

        Console.Write("Digite o número da opção desejada: ");

        string pagamento = Console.ReadLine();

        if (Enum.TryParse(pagamento, out Venda.Pagamento escolha))
        {
            Console.WriteLine($"Você escolheu: {escolha}");
        }
        else
        {
            Console.WriteLine("Opção inválida! Tente novamente!");
            SelecionaPagamento();
        }
    }
    #endregion

    #region Login
    public static void Login()
    {
        //Apresenta as opções de login ao usuário e chama seus respectivos login's
        //O login administrador é fixo, enquanto o usuário é lido dos usuários cadastrados no sistema

        Console.Clear();
        Console.WriteLine("Vendas de Cantina UnoChapeco");
        Console.WriteLine("Selecione a opção de login:");
        Console.WriteLine("1 - Usuário administrador");
        Console.WriteLine("2 - Usuário");

        int opcaoLoginUsu = int.Parse(Console.ReadLine());

        switch (opcaoLoginUsu)
        {
            case 1:
                LoginAdmin();
                break;

            case 2:
                LoginUsuario();
                break;
        }
    }
    public static void MenuPrincipal(string usuario)
    {
        //apresenta as funcionalidades do sistema e encaminha o usuário a escolher as mesmas
        //Cadastros - Para gravar Usuários e Produtos em arquivos para serem usados depois
        //Vendas - Para vender os produtos e gerar relatórios

        Console.Clear();
        Console.WriteLine($"Bem vindo ao sistema de vendas {usuario}!");
        Console.WriteLine("1 - Cadastros");
        Console.WriteLine("2 - Vendas");

        int opcaoMenu = int.Parse(Console.ReadLine());

        switch (opcaoMenu)
        {
            case 1:
                Console.WriteLine();
                MenuCadastros(usuario);
                break;

            case 2:
                MenuVendas(usuario);
                break;
        }
    }
    public static void LoginAdmin()
    {
        //Aqui é feito login do administrador do sistema, o login sempre será "admin" e a senha "adm123", foi definido por padrão
        //Se a pessoa errar o login, enquanto errar o login, vai cair no loop
        //Quando acertar, vai chamar o menu principal da aplicação

        Console.Clear();
        Console.WriteLine("Login: ");
        string loginUsuAdm = Console.ReadLine();
        Console.WriteLine("Senha: ");
        string senhaUsuAdm = Console.ReadLine();

        if (loginUsuAdm != "admin" && senhaUsuAdm != "adm123")
        {
            do
            {
                Console.WriteLine("Senha ou login incorretos! Tente novamente!");
                GravarLog("?", "Tentativa de logar como admin");
                Console.WriteLine("Vendas de Cantina UnoChapeco");
                Console.WriteLine("Login: ");
                loginUsuAdm = Console.ReadLine();
                Console.WriteLine("Senha: ");
                senhaUsuAdm = Console.ReadLine();

                if (loginUsuAdm == "admin" && senhaUsuAdm == "adm123")
                {
                    Console.WriteLine("Acesso (Administrador) liberado por login e senha");
                    GravarLog("admin", "Usuário admin fez login");
                    Console.ReadKey();
                    MenuPrincipal(loginUsuAdm);
                }

            } while (loginUsuAdm != "admin" && senhaUsuAdm != "adm123");
        }
        else
        {
            Console.WriteLine("Acesso (Administrador) liberado por login e senha");
            GravarLog("admin", "Usuário admin fez login");
            Console.ReadKey();
            MenuPrincipal(loginUsuAdm);
        }
    }
    public static void LoginUsuario()
    {
        //Aqui é feito o login de usuário cadastrado na aplicação, os usuários são gravados em arquivos e lidos para realizar o login de usuário
        Console.Clear();
        Console.WriteLine("Login: ");
        string loginUsu = Console.ReadLine();
        Console.WriteLine("Senha: ");
        string senhaUsu = Console.ReadLine();

        string diretorio = @"C:\SistemaDeVendas\Usuarios";
        string arquivo = "Usuarios.txt";
        string caminho = Path.Combine(diretorio, arquivo);

        string[] usuarios = File.ReadAllLines(caminho);

        var checaArquivo = new FileInfo(caminho);

        //Checa se tem dados gravados nos usuários, se não tiver volta a tela de login
        if (checaArquivo.Length == 0)
        {
            Console.WriteLine("Não existem usuários cadastrados!");
            Console.ReadKey();
            Login();
        }
        else
        {
            //Se tiver, como os dados são salvos em 2 linhas, vai ler o arquivo até encontrar um login e senha compatíveis com o informado e logar
            //Como o padrão para gravar o usuário e a senha é login e senha, o impar sempre será o login, e o par sempre será a senha
            //Por exemplo, João(1),joao123(2),Felipe(3),felipe123(4)

            int contador = 1;
            string login = "", senha = "";

            foreach (string usuario in usuarios)
            {
                if (contador % 2 == 0)
                {
                    //Lê a senha informada
                    senha = usuario.Trim();

                    if (login == loginUsu && senha == senhaUsu)
                    {
                        //Quando achar um usuário com login e senha iguais aos informados, chama o menu principal
                        Console.WriteLine($"Sistema acessado pelo usuário {login}");
                        GravarLog(login, $"Usuário {login} fez login");
                        Console.ReadKey();
                        MenuPrincipal(login);
                    }
                }
                else
                {
                    //Lê o usuario informado
                    login = usuario.Trim();
                }
                contador++;
            }
        }
        // Se caiu aqui, é por que leu todos os dados, e nenhum deles era um login correto, logo, oferece a opção de tentar novamente ou voltar a tela de login
        Console.WriteLine("Senha ou login inválidos! Deseja voltar a tela de login?");
        GravarLog("?", $"Tentativa de login com usuário: {loginUsu}");
        Console.WriteLine("1 - Sim");
        Console.WriteLine("2 - Tentar novamente");
        GravarLog("?", $"Tentativa de login com usuário: {loginUsu} e senha: {senhaUsu}");
        int opcaoLogin = int.Parse(Console.ReadLine());

        if (opcaoLogin == 1)
        {
            Login();
        }
        else
        {
            //Essa função é o mesmo código usado acima, só que é recursiva, para cair em loop caso o usuário erre n vezes
            NovaTentativa();
        }
    }
    public static void NovaTentativa()
    {
        //Mesmo codigo de validação do login de usuário, porém cai em loop com recursividade até acertar o login e logar
        Console.Clear();
        Console.WriteLine("Senha ou login incorretos, tente novamente!");
        Console.WriteLine("Login: ");
        string loginUsu = Console.ReadLine();
        Console.WriteLine("Senha: ");
        string senhaUsu = Console.ReadLine();

        string diretorio = @"C:\SistemaDeVendas\Usuarios";
        string arquivo = "Usuarios.txt";
        string caminho = Path.Combine(diretorio, arquivo);

        string[] usuarios = File.ReadAllLines(caminho);

        var checaArquivo = new FileInfo(caminho);

        if (checaArquivo.Length == 0)
        {
            Console.WriteLine("Não existem usuários cadastrados!");
            Console.ReadKey();
            Login();
        }
        else
        {
            int contador = 1;
            string login = "", senha = "";

            foreach (string usuario in usuarios)
            {
                if (contador % 2 == 0)
                {
                    senha = usuario.Trim();

                    if (login == loginUsu && senha == senhaUsu)
                    {
                        Console.WriteLine($"Sistema acessado pelo usuário {login}");
                        Console.ReadKey();
                        MenuPrincipal(login);
                    }
                }
                else
                {
                    login = usuario.Trim();
                }
                contador++;
            }
        }
        Console.WriteLine("Senha ou login inválidos! Deseja voltar a tela de login?");
        Console.WriteLine("1 - Sim");
        Console.WriteLine("2 - Tentar novamente");
        GravarLog("?", $"Tentativa de login com usuário: {loginUsu}");
        int opcaoLogin = int.Parse(Console.ReadLine());

        if (opcaoLogin == 1)
        {
            Login();
        }
        else
        {
            NovaTentativa();
        }
    }
    #endregion

    //Função que grava os log's
    public static void GravarLog(string usuario, string mensagem)
    {
        //Grava o log do que está sendo feito na aplicação
        //Parâmetros - usuário logado para identificar o usuário e o que acessou, e uma mensagem dependendo do que for o caso

        string diretorio = @"C:\SistemaDeVendas\Logs";
        string arquivo = "Logs.txt";
        string caminho = Path.Combine(diretorio, arquivo);

        try
        {
            if (!Directory.Exists(diretorio))
            {
                Directory.CreateDirectory(diretorio);
                Console.WriteLine("Diretório criado com sucesso!");
            }

            if (!File.Exists(caminho))
            {
                using (File.Create(caminho)) { }

                Console.WriteLine("Arquivo criado com sucesso!");
            }

            DateTime logData = DateTime.Now;

            string dados = $"{logData} - {usuario}: {mensagem}\n";
            File.AppendAllText(caminho, dados);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocorreu um erro com Logs: {ex.Message}, verifique!");
        }
    }
    //Função que converte o valor dos preços das vendas diárias em float para somá-las
    static float ExtrairValorNumericoComoFloat(string linha)
    {
        // Remove todos os caracteres não numéricos ou de ponto
        string valorString = new string(linha.Where(c => char.IsDigit(c) || c == ',' || c == '.').ToArray());

        // Substitui vírgulas por pontos (para formatos que usam vírgula como separador decimal)
        valorString = valorString.Replace(',', '.');

        // Converte para float
        if (float.TryParse(valorString, NumberStyles.Any, CultureInfo.InvariantCulture, out float valorFloat))
        {
            return valorFloat;
        }

        // Se não for possível converter, retorna 0
        return 0;
    }
}
