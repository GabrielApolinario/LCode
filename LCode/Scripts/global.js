window.onscroll = function() {
    menuSticky()
};
/*MENU*/
function menuSticky() {
    var navbar = document.querySelector("nav");
    if (window.pageYOffset >= 0.5) {
        navbar.classList.add("scrolling");
    } else {
        navbar.classList.remove("scrolling");
    }
}




function fnTabs(tab) {
    //Verifica se a tab que foi clicada está com a classe "tab-selected"
    if (!tab.classList.contains("tab-selected")) {
        //Se não estiver, verifica se o id do elemento clicado é igual a "lost-form"
        if (tab.id == "lost-form") {
            //Se for, adicionar a classe "hide" nas tabs
            document.querySelector(".tabs-select").classList.add("hide");
            //Remove a classe "tab-active" para retirar a tab que está selecionada
            document.querySelector(".tab-active").classList.remove("tab-active");
            //Adiciona a classe "tab-active" para dizer que tem uma tab ativa
            document.getElementById(tab.id + "-tab").classList.add("tab-active");
        } else {
            //Se não, verificar se existe alguma tab com a classe "hide"
            if (document.querySelector(".tabs-select").classList.contains("hide")) {
                //Se existir, remove a classe "hide"
                document.querySelector(".tabs-select").classList.remove("hide");
            }
            //Remove a classe "tab-active" da tab atual
            document.querySelector(".tab-active").classList.remove("tab-active");
            //Adiciona a classe "tab-active" para a tab clicada
            document.getElementById(tab.id + "-tab").classList.add("tab-active");
            //Verifica se a id da tab clicada está selecionada no selector de tabs
            if (!document.getElementById(tab.id).classList.contains("tab-selected")) {
                //Se não existir, remover a classe "tab-selected" da tab que está selecionada
                document.querySelector(".tab-selected").classList.remove("tab-selected");
                //Adiciona a classe "tab-selected" para tab clicalda
                document.getElementById(tab.id).classList.add("tab-selected");
            }
        }
    }
}

function fnBack() {
    //Adiciona a classe "hide" para o selector de tabs
    document.querySelector(".tabs-select").classList.remove("hide");
    //Remove a classe "tab-active" da tab esqueceu sua senha
    document.getElementById("lost-form-tab").classList.remove("tab-active");
    //Adiciona a classe "tab-active" para a tab login
    document.getElementById("login-form-tab").classList.add("tab-active");
}

