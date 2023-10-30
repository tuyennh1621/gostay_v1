class StickyNavigation {
	
	constructor() {
		this.currentId = null;
		this.currentTab = null;
		this.tabContainerHeight = 70;
		this.lastScroll = 0;
		let self = this;
		$('.sticky-nav-tab').click(function() { 
			self.onTabClick(event, $(this)); 
		});
		$(window).scroll(() => { this.onScroll(); });
		$(window).resize(() => { this.onResize(); });
	}
	
	onTabClick(event, element) {
		event.preventDefault();
		let scrollTop = $(element.attr('href')).offset().top - this.tabContainerHeight + 1;
		$('html, body').animate({ scrollTop: scrollTop }, 600);
	}
	
	onScroll() {
		this.checkHeaderPosition();
    this.findCurrentTabSelector();
		this.lastScroll = $(window).scrollTop();
	}
	
	onResize() {
		if(this.currentId) {
			this.setSliderCss();
		}
	}
	
	checkHeaderPosition() {
		const headerHeight = 75;
		if($(window).scrollTop() > headerHeight) {
			$('.spa-header').addClass('spa-header--scrolled');
		} else {
			$('.spa-header').removeClass('spa-header--scrolled');
		}
		let offset = ($('.sticky-nav-tabs').offset().top + $('.sticky-nav-tabs').height() - this.tabContainerHeight) - headerHeight;
		if($(window).scrollTop() > this.lastScroll && $(window).scrollTop() > offset) {
			$('.spa-header').addClass('spa-header--move-up');
			$('.sticky-nav-tabs-container').removeClass('sticky-nav-tabs-container--top-first');
			$('.sticky-nav-tabs-container').addClass('sticky-nav-tabs-container--top-second');
		} 
		else if($(window).scrollTop() < this.lastScroll && $(window).scrollTop() > offset) {
			$('.spa-header').removeClass('spa-header--move-up');
			$('.sticky-nav-tabs-container').removeClass('sticky-nav-tabs-container--top-second');
			$('.sticky-nav-tabs-container').addClass('sticky-nav-tabs-container--top-first');
		}
		else {
			$('.spa-header').removeClass('spa-header--move-up');
			$('.sticky-nav-tabs-container').removeClass('sticky-nav-tabs-container--top-first');
			$('.sticky-nav-tabs-container').removeClass('sticky-nav-tabs-container--top-second');
		}
	}
	
	findCurrentTabSelector(element) {
		let newCurrentId;
		let newCurrentTab;
		let self = this;
		$('.sticky-nav-tab').each(function() {
			let id = $(this).attr('href');
			let offsetTop = $(id).offset().top - self.tabContainerHeight;
			let offsetBottom = $(id).offset().top + $(id).height() - self.tabContainerHeight;
			if($(window).scrollTop() > offsetTop && $(window).scrollTop() < offsetBottom) {
				newCurrentId = id;
				newCurrentTab = $(this);
			}
		});
		if(this.currentId != newCurrentId || this.currentId === null) {
			this.currentId = newCurrentId;
			this.currentTab = newCurrentTab;
			this.setSliderCss();
		}
	}
	
	setSliderCss() {
		let width = 0;
		let left = 0;
		if(this.currentTab) {
			width = this.currentTab.css('width');
			left = this.currentTab.offset().left;
		}
		$('.sticky-nav-tab-slider').css('width', width);
		$('.sticky-nav-tab-slider').css('left', left);
	}
	
}

new StickyNavigation();

var _gaq = _gaq || [];
  _gaq.push(['_setAccount', 'UA-36251023-1']);
  _gaq.push(['_setDomainName', 'jqueryscript.net']);
  _gaq.push(['_trackPageview']);

  (function() {
    var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
    ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
    var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
  })();

