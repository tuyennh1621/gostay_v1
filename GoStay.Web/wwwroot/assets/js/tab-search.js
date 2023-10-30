function openSearch(evt, cityName) {
  var i, tabcontent2, tablinks1;
  tabcontent2 = document.getElementsByClassName("tabcontent2");
  for (i = 0; i < tabcontent2.length; i++) {
    tabcontent2[i].style.display = "none";
  }
  tablinks1 = document.getElementsByClassName("tablinks1");
  for (i = 0; i < tablinks1.length; i++) {
    tablinks1[i].className = tablinks1[i].className.replace(" active", "");
  }
  document.getElementById(cityName).style.display = "block";
  evt.currentTarget.className += " active";
}

function TabComment(evt, cityName) {
  var i, tabcontent2, tablinks1;
  tabcontent2 = document.getElementsByClassName("tabcontent2");
  for (i = 0; i < tabcontent2.length; i++) {
    tabcontent2[i].style.display = "none";
  }
  tablinks1 = document.getElementsByClassName("tablinks1");
  for (i = 0; i < tablinks1.length; i++) {
    tablinks1[i].className = tablinks1[i].className.replace(" active", "");
  }
  document.getElementById(cityName).style.display = "block";
  evt.currentTarget.className += " active";
}

var _defaultOpen2 = document.getElementById("defaultOpen2");
if (_defaultOpen2) {
    _defaultOpen2.click();
}

var _defaultOpen = document.getElementById("defaultOpen");
if (_defaultOpen) {
    _defaultOpen.click();
}
  /* Get the element with id="defaultOpen" and click on it */
  //document.getElementById("defaultOpen").click();
 /* document.getElementById("defaultOpen2").click();*/

$("#txtInputHotelSearch").click(function () {
  $("#result_search_hotel").show();
});

$("#txtInputRoomSearch").click(function () {
  $("#result_search_room3").show();
});

$("#txtInputDateSearch").click(function () {
  $("#result_search_date").show();
});

$("#txtInputDateSearch1").click(function () {
  $("#result_search_plane_date").show();
});

$("#txtInputPlaneSearch").click(function () {
  $("#result_search_plane_departure").show();
});

$("#txtInputPlaneSearch1").click(function () {
  $("#result_search_plane_arrival").show();
});

$("#txtInputSeatSearch").click(function () {
  $("#result_search_seat").show();
});

$("#txtInputTourSearch").click(function () {
  $("#result_search_tour").show();
});

$("#txtInputTourDateSearch").click(function () {
  $("#result_search_tour_date").show();
});

$("#txtInputTourPerson").click(function () {
  $("#result_search_tour_person").show();
});


$(document).mouseup(function(e) 
{
    
var container_result_search_hotel = $("#result_search_hotel");

// if the target of the click isn't the container nor a descendant of the container
  if (!container_result_search_hotel.is(e.target) && container_result_search_hotel.has(e.target).length === 0) 
  {
    container_result_search_hotel.hide();
  }
});
    
$(document).mouseup(function(e) 
{
    
var container_result_search_room3 = $("#result_search_room3");

// if the target of the click isn't the container nor a descendant of the container
  if (!container_result_search_room3.is(e.target) && container_result_search_room3.has(e.target).length === 0) 
  {
    container_result_search_room3.hide();
  }
});

$(document).mouseup(function(e) 
{
    
var container_result_search_date = $("#result_search_date");

// if the target of the click isn't the container nor a descendant of the container
  if (!container_result_search_date.is(e.target) && container_result_search_date.has(e.target).length === 0) 
  {
    container_result_search_date.hide();
  }
});

$(document).mouseup(function(e) 
{
    
var container_result_search_plane_date = $("#result_search_plane_date");

// if the target of the click isn't the container nor a descendant of the container
  if (!container_result_search_plane_date.is(e.target) && container_result_search_plane_date.has(e.target).length === 0) 
  {
    container_result_search_plane_date.hide();
  }
});


$(document).mouseup(function(e) 
{
    
var container_result_search_plane_departure = $("#result_search_plane_departure");

// if the target of the click isn't the container nor a descendant of the container
  if (!container_result_search_plane_departure.is(e.target) && container_result_search_plane_departure.has(e.target).length === 0) 
  {
    container_result_search_plane_departure.hide();
  }
});

$(document).mouseup(function(e) 
{
    
var container_result_search_plane_arrival = $("#result_search_plane_arrival");

// if the target of the click isn't the container nor a descendant of the container
  if (!container_result_search_plane_arrival.is(e.target) && container_result_search_plane_arrival.has(e.target).length === 0) 
  {
    container_result_search_plane_arrival.hide();
  }
});

$(document).mouseup(function(e) 
{
    
var container_result_search_seat = $("#result_search_seat");

// if the target of the click isn't the container nor a descendant of the container
  if (!container_result_search_seat.is(e.target) && container_result_search_seat.has(e.target).length === 0) 
  {
    container_result_search_seat.hide();
  }
});

$(document).mouseup(function(e) 
{
    
var container_result_search_tour = $("#result_search_tour");

// if the target of the click isn't the container nor a descendant of the container
  if (!container_result_search_tour.is(e.target) && container_result_search_tour.has(e.target).length === 0) 
  {
    container_result_search_tour.hide();
  }
});

$(document).mouseup(function(e) 
{
    
var container_result_search_tour_date = $("#result_search_tour_date");

// if the target of the click isn't the container nor a descendant of the container
  if (!container_result_search_tour_date.is(e.target) && container_result_search_tour_date.has(e.target).length === 0) 
  {
    container_result_search_tour_date.hide();
  }
});

$(document).mouseup(function(e) 
{
    
var container_result_search_tour_person = $("#result_search_tour_person");

// if the target of the click isn't the container nor a descendant of the container
  if (!container_result_search_tour_person.is(e.target) && container_result_search_tour_person.has(e.target).length === 0) 
  {
    container_result_search_tour_person.hide();
  }
});