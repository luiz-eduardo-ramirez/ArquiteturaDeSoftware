# Documentacao Técnica

## **1** Ferramentas e processos

### **1.1** Versionamento de arquivos
### **1.2** Estilo de programação
### **1.3** Processos organizacionais

---

## **2** Arquitetura

![](VistaLogica_Legendas.png)

Abaixo está descrita a vista lógica da aplicação em um esquema produzido a partir de uma linguagem criada pela a equipe.

### **2.2** Definições
![](VistaLogica_Definicoes.png)

//Descrever quais são os elementos definidos

### **2.3** Visão de Baralho
![](VistaLogica_VisaoDeBaralho.png)

//Descrever rapidamente que nessa tela podem ser criados, editados e deletados os 
**Flashcards** 

### **2.4** Visão de Aprendizado
![](VistaLogica_VisaoDeAprendizado.png)

(Errata: O esquema possui um erro onde **Flashcards** pertencentes a sessão de aprendizado avançado e que foram esquecidos regridem para a etapa de conhecimento superficial. Na realidade, o **Flashcard** deve retornar para a etapa de ignorância)

//Descrever as etapas que irão existir e sistema de leitner

### **2.5** Envio de **Flashcards**
![](VistaLogica_EnvioDeFlashcards.png)

//Descrever como o  sistema de "merge" de baralho ira funcionar e o sistema de comparilhamento de baralhos

---

### **3** Soluções para os requisitos da aplicação:
1. Carregar um conjunto contendo 10 **Flashcards**, pelo menos;
    
    Quando a aplicação for iniciada no dispositivo do usuário, uma requisição será feita para o servidor, de modo que, a existência do registro do id do usuário (ID único de dispositivo) será verificada. Caso o registro exista, a aplicação tentará carregar o baralho correspondente a esse usuário. Caso ele não exista, o usuário será registrado no servidor.
    
    O usuário só poderá entrar em sessões de aprendizado caso seu baralho tenha mais de 10 **Flashcards**. Se não for o caso, o usuário só pode entrar na visão de baralho, onde ele poderá adicionar mais **Flashcards**.

2. Virar um **Flashcard** a cada solicitação do usuário;

    Quando o usuario acessa a tela de escolha de menu de aprendizagem, ele terá a visão de baralho em que haverá um **Flashcards**, sendo que, um **Flashcards** irá possuir um termo, o qual é a materialização de um item de conhecimento que é definida pela caracterização do item de conhecimento.

    //dentro das visoes de baralho e aprendizado isso é possivel, explicar termo e definicao

3. Passar para outro **Flashcard** a pedido do usuário;

    A aplicação implementa uma roda mágica que consiste em organizar os **Flashcards** em formato de anel onde o usuário poderá percorrer pelas cartas e escolher seu **Flashcard**

    ////dentro das visoes de baralho e aprendizado isso é possivel, explicar a roda magica? falar que na visao de aprendizado tbm eh possivel pesquisar flashcards

4. Sortear um **Flashcard**, quando o usuário desejar;

    Dentro das condições de que o usuário está cadastrado e que possui um baralho válido, ele poderá assumir a visão de aprendizado em que o usuário poderá escolher entre 3 sessões, caso elas estejam populadas por **Flashcards**, essas sessões são: Ignorância, conhecimento parcial e conhecimento completo. Quando uma sessão é escolhida um dos **Flashcards** que ela possui é sorteado para ser avaliado.
    //descrever o sorteio na visao de aprendizado

5. Mostrar quantos **Flashcards** o usuário corretamente se lembrou;

    Em uma sessão de aprendizado, após o usuário terminar a avaliação dos **Flashcards**, ele poderá encerrar a sessão. Quando uma sessão é encerrada, a aplicação mostrará para o usuário quantos flashcards ele se lembrou. Em seguida os **Flashcards** lembrados passarão para próxima sessão e os esquecidos voltarão para sessão de ignorância. 
 
    //descrever interface na visao de aprendizado
    
6. Suportar a inserção e remoção de **Flashcards**;

    O usuário, caso possua um baralho, pode entrar na visão de baralho onde ele vai ter a possibilidade de percorrer o mesmo, inserir ou remover um **Flashcard**, virar um  **Flashcard**, e buscar um **Flashcard**.

    //descrever visao de baralho

7. Ser executado em desktops e celulares;

    A ferramenta de desenvolvimento Unity que usamos no projeto tem suporte a portabilidade entre dispositivos e sistemas diferentes sem a necessidade de uma alteração na arquitetura ou desenvolvimento.

    //Uso da ferramenta unity e arquitetura com cliente e servidor

8. Permitir que um usuário troque **Flashcards** com outro usuário;

    A ferramenta de desenvolvimento Unity fornece uma funcionalidade de geração de ID único para cada usuário, baseado em sua máquina. Os ID serão utilizados no cadastro e reconhecimento de usuários, além da identificação de seus baralhos, assim possibilitando sua cópia (troca de baralho) através do uso de um servidor AWS.

    //Descrever como um usuario pode ser selecionado pelo seu id unico para absorção de baralho (tela de troca de baralho)

9. A aplicação deverá “animar” um **Flashcard** sempre que ele for virado;

    A ferramenta de desenvolvimento Unity dá suporte a animações e interações gráficas da aplicação.

    //UNITy da suporte a animacao

10. Basear-se na proposta de Leitner1 quando fizer o sorteio de **Flashcards**.

    A proposta de Leitner se resume em um método de memorização através de **Flashcards** onde existem caixas que dividem os **Flashcards** em níveis de aprendizado, a cada sessão de estudo o estudante verifica a caixa correspondente a sessão, além das anteriores a mesma e passa os **Flashcards** que lembra para caixa seguinte a qual eles se encontram e os que esquece a primeira caixa respectivamente, esse processo é repetido até todos **Flashcards** serem memorizados, ou seja, chegarem na última caixa.

    Em nosso projeto, instauramos uma visão do modelo de Leitner em que existem 3 caixas, ou sessões de aprendizado (ignorância, parcial e completo), em que cada sessão o usuário marca se lembrou ou não dos conceitos do **Flashcard** e após marcar todos e finalizar a sessão, eles são movidos para próxima caixa ou para primeira dependendo de suas respetivas marcações.
    
    //Descrever leitner