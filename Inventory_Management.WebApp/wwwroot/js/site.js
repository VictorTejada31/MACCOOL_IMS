// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//Index Cashier


//Table Filter
const INPUT = document.getElementById('seachTable');

INPUT.addEventListener('keyup', () => {
    var filter, table, tr, td, i, txtValue;
    filter = INPUT.value.toUpperCase();
    table = document.getElementById("myTable");
    tr = table.getElementsByTagName("tr");

    for (i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[1];
        if (td) {
            txtValue = td.textContent || td.innerText;
            if (txtValue.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
});

//OnClick Event
function removeFromList(target) {
    const CardProductContainer = document.getElementById(target);
    console.log(document.getElementById('bills-Product-6'));
    console.log(target);

    CardProductContainer.remove();
}

//Remove products

const BtnRemove = document.getElementById('btn_remove_products');
const BillContainer = document.getElementById('bills-Product-Container');



BtnRemove.addEventListener('click', () => {
    BillContainer.innerHTML = '';
});


//Add Other Modal

const OtherModalForm = document.getElementById('OtherModalForm');

OtherModalForm.addEventListener('submit', () => {
    $('#addNewProduct').modal('hide');
});


//Show/Hide func

function HideShowProductModal(isSearching) {
    if (isSearching) {
        Product_Container.hidden = false;
        AddProduct_Container.setAttribute('hidden', true);
    }
    else {
        Product_Container.hidden = true;
        AddProduct_Container.removeAttribute('hidden');
    }
}

//Show/Hide Alert

function ShowHideAlert() {

    Alert_ProductNotFound.removeAttribute('hidden');
    setTimeout(() => {
        Alert_ProductNotFound.setAttribute('hidden', true);
    }, 3000)
}


//Add to Bill List
const BtnAddToBill = document.getElementById('card_product_addtobill');
BtnAddToBill.addEventListener('click', () => {
    AddToList(_id, _name, _price);
    HideShowProductModal(false);
});



//Get Total Func

function GetBillTotal() {

    let total = 0;

    for (const child of Bill_Container.children) {
        let strSplit = child.children[0].textContent.split("-");
        let num = parseInt(strSplit[strSplit.length - 1].replace("$", "").trim());
        total += num;

    }

    return total;

}


const Btnbillforclient = document.getElementById('btnBillForClient');

Btnbillforclient.addEventListener('click', () => {
    let clientId = document.getElementById('selectClient').value;
    ProcessBillClient(clientId, GetBillTotal());
    document.getElementById('selectClient').value = "0";
    BillContainer.innerHTML = "";
    setTimeout(() => {
        location.reload();
    },1100);
});

//Seach Input





