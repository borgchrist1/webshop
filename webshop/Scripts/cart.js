'use strict';
let cart = [];

function onClick(event) {
    let id = event.srcElement.dataset.set;
    fetchJson("http://localhost:61176/home/AddToCart/" + id);

}

function fetchJson(url) {
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            cart = JSON.parse(this.responseText);
            console.log(cart);
            updateCart(cart);
        }
    };
    xhttp.open("GET", url, true);
    xhttp.send();

}

function fetchCart() {
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            if (cart !== null) {
                cart = JSON.parse(this.responseText);
                console.log(cart);
                updateCart(cart);
            }
        }
    };
    xhttp.open("GET", 'http://localhost:61176/home/getcart', true);
    xhttp.send();
}

function getCart() {
    console.log('hello');
    fetchCart();
    if (cart !== null) {
        updateCart(cart);
    }
}

function updateCart(cart) {
    console.log(cart);

    let cartDropDown = document.querySelector('#cart');
    let badge = document.querySelector('.badge');
    badge.innerText = cart.length;

}

let add = document.querySelectorAll('.add');
for (var i = 0; i < add.length; i++) {
    add[i].addEventListener('click', onClick);
}

fetchCart();
