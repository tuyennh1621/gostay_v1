function openFilter(evt, cityName){
  var i, content_listing_page, filtertop;
  content_listing_page = document.getElementsByClassName("content_listing_page");
  for (i = 0; i < content_listing_page.length; i++) {
    content_listing_page[i].style.display = "none";
  }
  filtertop = document.getElementsByClassName("filtertop");
  for (i = 0; i < filtertop.length; i++) {
    filtertop[i].className = filtertop[i].className.replace(" active", "");
  }
  document.getElementById(cityName).style.display = "block";
  evt.currentTarget.className += " active";

}



