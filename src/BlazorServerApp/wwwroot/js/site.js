function saveMessage(firstName, lastName) {
    //alert(firstName + ' ' + lastName + ' has been saved successfully!');

    document.getElementById('divServerValidations').innerText = firstName + ' ' + lastName + ' has been saved successfully!';
}

function setFocusOnElement(element) {
    element.focus();
}

function getCities() {    

    //throw 'Something has gone wrong JS';

    var cities = ['New York', 'Los Angeles', 'Chicago', 'Houston', 'Phoenix', 'Philadelphia', 'San Antonio',
        'San Diego', 'Dallas', 'San Jose', 'Austin', 'Jacksonville', 'Fort Worth', 'Columbus', 'San Francisco',
        'Charlotte', 'Indianapolis', 'Seattle', 'Denver', 'Washington'];
    return cities;
}

window.SetDomTitle = (title) => {
    document.title = title;
};

window.GerGreetings = () => {
    var myDate = new Date();
    var hrs = myDate.getHours();

    var greet='';

    if (hrs < 12)
        greet = 'Good Morning';
    else if (hrs >= 12 && hrs <= 17)
        greet = 'Good Afternoon';
    else if (hrs >= 17 && hrs <= 24)
        greet = 'Good Evening';

    return greet;
    
};

window.TogglePannel = () => {
    var element = document.getElementById("myDIV");
    element.classList.toggle("mystyle");
};


window.CollapseExpand = (istoggled, MainSection, ToggledSection) => {

    if (istoggled == false) {
        $('#' + ToggledSection).css('display', 'block');
        $("#" + MainSection).removeClass("collapseClick");
        $("#" + MainSection).addClass("expandClick");
        
    }
    else if (istoggled == true) {
        $('#' + ToggledSection).css('display', 'none');
        $("#" + MainSection).removeClass("expandClick");
        $("#" + MainSection).addClass("collapseClick");
       
    }

};

window.ShowDiv = (section_div) => {
    
    $("#" + section_div).removeClass("d-none");
        
    
};

window.HideDiv = (section_div) => {
   
    $("#" + section_div).addClass("d-none");

};

window.highlightInput = (inputWrapper) => {
    var input = inputWrapper.querySelector("input");
    if (!input) return;
    input.select();
}

