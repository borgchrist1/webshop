'use strict';
let cart = [];
//let product = {};

function onClick(event) {
    let id = event.srcElement.dataset.set;
    fetchJson("http://localhost:61176/home/AddToCart/" + id);

}

function fetchJson(url) {
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            let product = JSON.parse(this.responseText);
            let addCart = [];
            cart.push(product);
            addCart.push(product);
            updateCart(addCart);
            console.log(product);
        }
    };
    xhttp.open("GET", url, true);
    xhttp.send();

}

function fetchCart() {
    var xhttp = new XMLHttpRequest();
        xhttp.onreadystatechange = function (){
        if (this.readyState == 4 && this.status == 200) {
            if (cart !== null) {
            let cart = JSON.parse(this.responseText);
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
    } //else {
    //  cart = fetchJson("http://localhost:61176/home/cart");
    //}
    //console.log(cart);
}

function updateCart(addCart) {
    console.log('hello');
    let cartDropDown = document.querySelector('#cart');
    let badge = document.querySelector('.badge');
    if (cart !== null) {
        badge.innerText = cart.length;
    } else {
        badge.innerText = addCart.length;
    }

    for (var i = 0; i < addCart.length; i++) {
        let listItem = document.createElement("li");
        let divider = document.createElement("li");
        divider.setAttribute('role', 'separator');
        divider.setAttribute('class', 'divider')
        listItem.innerText = addCart[i].Product.Name + ' ' + addCart[i].Product.Price + ' Kr';
        cartDropDown.appendChild(listItem);
        cartDropDown.appendChild(divider);
    }
}

let add = document.querySelectorAll('.add');
for (var i = 0; i < add.length; i++) {
    add[i].addEventListener('click', onClick);
}

fetchCart();
