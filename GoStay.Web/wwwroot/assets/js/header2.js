window.onscroll = function() {myFunction()};

var header2 = document.getElementById("header2_sticky");
var sticky = header2.offsetTop;

function myFunction() {
  if (window.pageYOffset > sticky) {
    header2.classList.add("sticky");
  } else {
    header2.classList.remove("sticky");
  }
}