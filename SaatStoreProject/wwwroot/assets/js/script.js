//slicky index.html section1slider
$('.slickslider').slick({
    slidesToShow: 1,
    slidesToScroll: 1,
    arrows: true,
    loop: true,
    draggable: true,
    infinite: true,
    cssEase: "linear",
    nextArrow: `<i class="fal fa-arrow-right iconright"></i>`,
    prevArrow: `<i class="fal fa-arrow-left iconleft"></i>`,
});


//watch-details slider
$('.slider-for').slick({
    slidesToShow: 1,
    slidesToScroll: 1,
    arrows: false,
    fade: true,
    asNavFor: '.slider-nav'
  });
  $('.slider-nav').slick({
    slidesToShow: 3,
    slidesToScroll: 1,
    asNavFor: '.slider-for',
    dots: true,
    focusOnSelect: true
  });
 
  $('a[data-slide]').click(function(e) {
    e.preventDefault();
    var slideno = $(this).data('slide');
    $('.slider-nav').slick('slickGoTo', slideno - 1);
  });

//watch-to details characteristcs
const element = document.getElementById('watch-details-attributes');

function scrollTomenu() {
  element.scrollIntoView(true);
}

//basketlist hover
$(".cart-box .header-cart-index").hover(function () {
    $(".cart-box .basketList").css("height", "285px")
}, function () {
        $(".basketAlert").css("opacity", "0%")
    $(".cart-box .basketList").css("height", "0%")
    }
)

$(".cart-box .basketList").hover(function () {
    $(this).css("height", "285px")
}, function () {
    $(this).css("height", "0%")
})

//watch filter checkbox clicks
$(function () {
    var span = $('span');
    //span click 
    span.on('click', function () {
        var checkbox = $(this).find('input[type="checkbox"]');
        checkbox.prop('checked', !checkbox.prop('checked'))
    });
    //checkbox click 
    $('input[type="checkbox"]').on('click', function () {
        $(this).prop('checked', !$(this).prop('checked'))
    });
});

//watch filter getsortby element
$(function getselectValue() {
    var selectedvalue = document.getElementById("list").getselectValue;
    console.log(selectedvalue);
})





//add to cart animation

const { to, registerPlugin, set, timeline } = gsap
gsap.registerPlugin(MorphSVGPlugin, Physics2DPlugin)

//gsap.globalTimeline.timeScale(.1);

document.querySelectorAll('.add-to-cart').forEach(button => {
    let background = button.querySelector('.background path')
    button.addEventListener('pointerdown', e => {
        if (button.classList.contains('active')) {
            return
        }
        to(button, {
            '--background-s': .97,
            duration: .1
        })
    })
    button.addEventListener('click', e => {
        e.preventDefault()
        if (button.classList.contains('active')) {
            return
        }
        button.classList.add('active')
        to(button, {
            keyframes: [{
                '--background-s': .97,
                duration: .1
            }, {
                '--background-s': 1,
                delay: .1,
                duration: .8,
                ease: 'elastic.out(1, .6)'
            }]
        })
        to(button, {
            '--text-x': '16px',
            '--text-o': 0,
            duration: .2
        })
        to(button, {
            keyframes: [{
                '--cart-x': '-12px',
                '--cart-s': 1,
                duration: .25
            }, {
                '--bottle-s': 1,
                '--bottle-o': 1,
                duration: .15,
                onStart() {
                    to(button, {
                        duration: .4,
                        keyframes: [{
                            '--bottle-r': '-8deg'
                        }, {
                            '--bottle-r': '8deg'
                        }, {
                            '--bottle-r': '0deg'
                        }]
                    })
                }
            }, {
                '--bottle-y': '0px',
                duration: .3,
                delay: .15,
                onStart() {
                    to(background, {
                        keyframes: [{
                            morphSVG: 'M0 19C0 10.7157 6.71573 4 15 4H41.4599C53.6032 4 62.4844 12 72.5547 12C82.6251 12 91.5063 4 103.65 4H137C139.761 4 142 6.23858 142 9V31C142 39.2843 135.284 46 127 46H5C2.23858 46 0 43.7614 0 41V19Z',
                            duration: .1,
                            delay: .18
                        }, {
                            morphSVG: 'M0 19C0 10.7157 6.71573 4 15 4H41.4599C53.6032 4 62.4844 4 72.5547 4C82.6251 4 91.5063 4 103.65 4H137C139.761 4 142 6.23858 142 9V31C142 39.2843 135.284 46 127 46H5C2.23858 46 0 43.7614 0 41V19Z',
                            duration: .8,
                            ease: 'elastic.out(1, .6)'
                        }]
                    })
                    to(button, {
                        '--bottle-s': .5,
                        duration: .15
                    })
                }
            }, {
                '--cart-y': '3px',
                duration: .1,
                onStart() {
                    to(button, {
                        keyframes: [{
                            '--check-y': '24px',
                            '--check-s': 1,
                            duration: .25
                        }, {
                            '--check-offset': '0px',
                            duration: .25
                        }]
                    })
                }
            }, {
                '--cart-y': '0px',
                duration: .2
            }, {
                '--cart-x': '-6px',
                '--bottle-r': '12deg',
                '--bottle-x': '-25%',
                duration: .15
            }, {
                '--cart-x': '-16px',
                '--bottle-r': '-12deg',
                '--bottle-x': '-50%',
                duration: .2,
                onStart() {
                    drops(button, 5, -130, -100);
                },
            }, {
                '--cart-x': '92px',
                '--cart-r': '-20deg',
                duration: .4,
                onStart() {
                    button.classList.add('clipped')
                },
                onComplete() {
                    set(button, {
                        '--cart-x': '-120px',
                        '--cart-s': .8,
                        '--cart-y': '-2px',
                        '--bottle-o': 0,
                        '--text-x': '2px'
                    })
                }
            }, {
                '--cart-x': '-57px',
                '--cart-r': '0deg',
                '--check-s': 0,
                duration: .3,
                delay: .1,
                clearProps: true,
                onStart() {
                    to(button, {
                        '--text-x': '10px',
                        '--text-o': 1,
                        duration: .2,
                        delay: .1
                    })
                },
                onComplete() {
                    button.classList.remove('active', 'clipped')
                }
            }]
        })
    })
})

function drops(parent, quantity, minAngle, maxAngle) {
    for (let i = quantity - 1; i >= 0; i--) {
        let angle = gsap.utils.random(minAngle, maxAngle),
            velocity = gsap.utils.random(60, 80)

        let div = document.createElement('div')
        div.classList.add('drop')

        parent.appendChild(div);

        set(div, {
            opacity: 1,
            scale: 0,
        });
        timeline({
            onComplete() {
                div.remove();
            }
        }).to(div, {
            duration: .4,
            scale: gsap.utils.random(.5, .7)
        }, 0).to(div, {
            duration: 1,
            physics2D: {
                angle: angle,
                velocity: velocity,
                gravity: 80
            }
        }, 0).to(div, {
            duration: .3,
            opacity: 0
        }, .3);
    }
}
