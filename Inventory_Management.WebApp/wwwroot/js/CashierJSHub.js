const CashierConnectionHub = new signalR.HubConnectionBuilder().withUrl("/hubs/cashierHub").build();

const Product_Container = document.getElementById('productCard_Container');
const Alert_ProductNotFound = document.getElementById('alert-danger-addProduct');
const ProductSearchContainer = document.getElementById('addProduct_Container');
const AddProduct_Container = document.getElementById('addProduct_Container');
const Bill_Container = document.getElementById('bills-Product-Container');
const Card_Product_Name = document.getElementById('card_product_name');
const Card_Product_Text = document.getElementById('card_product_text');
const Btn_bill = document.getElementById('btn_bill');
var _id, _name, _price;
var productsArray = [];



Btn_bill.addEventListener('click', () => {

    for (const child of Bill_Container.children) {
        let strSplit = child.children[0].textContent.split("-");
        productsArray.push([child.children[2].textContent, strSplit[0], strSplit[strSplit.length - 1]]);
    }

    BillContainer.innerHTML = '';
    ProcessBill();

});

function ProcessBillClient(clientId, total) {
    CashierConnectionHub.send("ProcessBillClients", clientId, total);
}


function ShowProductInfo(imgPath, name, desc, category, price) {

    let img = document.getElementById('card_product_img');
    img.setAttribute('src', imgPath);
    console.log(img);
    Card_Product_Name.innerText = name;
    Card_Product_Text.innerHTML = `${desc} 
        <span class="code-bar-span" id="card_product_category"><b>Categoría:</b>${category}</span>
         <span class="code-bar-span" id="card_product_price">
            <b>Precio:</b> <span style="color:green">
            $<span style="color:black">${price}</span></span>
         </span>`;

    HideShowProductModal(true);

}
function AddToList(id, name, price, isOther) {

    let ContainerDiv = document.createElement('div');
    let LeftDiv = document.createElement('div');
    let RightDiv = document.createElement('div');
    let Span = document.createElement('span');
    let Button = document.createElement('button');

    LeftDiv.setAttribute('class', 'bills_left');
    RightDiv.setAttribute('id', 'bills_right');
    Button.setAttribute('class', 'btn btn-danger btn-sm');
    Button.setAttribute('type', 'button')
    Span.setAttribute('hidden', true);

    if (isOther == true) {
        ContainerDiv.setAttribute('id', `bills-Product-${name}`);
        Button.setAttribute('onclick', `removeFromList("bills-Product-${name}")`);
        console.log("ver");


    }
    else {
        ContainerDiv.setAttribute('id', `bills-Product-${id}`);
        Button.setAttribute('onclick', `removeFromList("bills-Product-${id}")`);
        Span.innerText = id;
        console.log("deja");
    }

    LeftDiv.innerHTML = `<strong>${name}</strong> ------------------------------------------------------------------ $${price}`;
    Button.innerText = "Remover";


    ContainerDiv.appendChild(LeftDiv);
    RightDiv.appendChild(Button);
    ContainerDiv.appendChild(RightDiv);
    ContainerDiv.appendChild(Span);
    Bill_Container.appendChild(ContainerDiv);
}

function ProcessBill() {
    console.log(productsArray);
    CashierConnectionHub.send("ProcessBill", productsArray);

}

CashierConnectionHub.on("AddNewProduct", (id, name, price, desc, category, img) => {
    if (id == 0) {
        ShowHideAlert();
    }
    else {

        ShowProductInfo(img, name, desc, category, price);
        _id = id;
        _name = name;
        _price = price;

    }
});

CashierConnectionHub.on("AddNewProductOther", (name, price) => {
    AddToList(0, name, price, true);
});




function fulfilled() {
    console.log("CashierHub connection is fulfilled");
}

function rejected() {
    console.log("CashierHub connection has been rejected");
}

CashierConnectionHub.start().then(fulfilled, rejected);