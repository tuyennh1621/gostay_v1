function openFilter(evt, cityName){
    var i, reviews_listing, filtercenter;
    reviews_listing = document.getElementsByClassName("reviews_listing");
    for (i = 0; i < reviews_listing.length; i++) {
      reviews_listing[i].style.display = "none";
    }
    filtercenter = document.getElementsByClassName("filtercenter");
    for (i = 0; i < filtercenter.length; i++) {
      filtercenter[i].className = filtercenter[i].className.replace(" active", "");
    }
    document.getElementById(cityName).style.display = "block";
    evt.currentTarget.className += " active";
  }
  
  // Get the element with id="defaultOpen" and click on it
  document.getElementById("defaultOpen").click();