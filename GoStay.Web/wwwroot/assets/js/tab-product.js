function openCity(evt, cityName, mainTab) {
  var i, tabcontent_hotel, tablinks;
  
 // tabcontent = document.getElementById("tabcontent_hotel");

 tabcontent_hotel = document.getElementById(mainTab).getElementsByClassName("tabcontent_hotel");
  console.log('tabcontent_hotel'+ tabcontent_hotel.length);

  for (i = 0; i < tabcontent_hotel.length; i++) {
    tabcontent_hotel[i].style.display = "none";
  }

  tablinks = document.getElementsByClassName("tablinks");
  for (i = 0; i < tablinks.length; i++) {
    tablinks[i].className = tablinks[i].className.replace(" active", "");
  }
  document.getElementById(cityName).style.display = "block";
  evt.currentTarget.className += " active";

}

   // Get the element with id="defaultOpen" and click on it
   // document.getElementById("defaultOpen").click();




