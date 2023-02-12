
const fullname = document.getElementById('fullname');
const email = document.getElementById('email');
const phone = document.getElementById('phone');
const address = document.getElementById('address');

const city = document.getElementById('province'); //thanh pho
const district = document.getElementById('district'); //quan/huyen
const ward = document.getElementById('village');//

fetch('/data.json')
    .then((response) => response.json())
    .then((data) => renderCity(data));

function renderCity(data) {
    for (var item of data) {
        city.options[city.options.length] = new Option(item.Name, item.Id);
    }

   
    city.onchange = () => {
        district.length = 1;
   
 
        if (city.value != '') {
           
            const result = data.filter((n) => n.Id === city.value);
            
            for (var item of result[0].Districts) {
                district.options[district.options.length] = new Option(
                    item.Name,
                    item.Id
                );
            }
        } else {
            
        }
    };

    district.onchange = () => {
        ward.length = 1;
        const result = data.filter((el) => el.Id === city.value);
        if (district.value != ' ') {
            const resultDistrict = result[0].Districts.filter(
                (el) => el.Id === district.value
            );

            for (var item of resultDistrict[0].Wards) {
                ward.options[ward.options.length] = new Option(item.Name, item.id);
            }
        }
    };
}
// renderCity(dt);
import validation from './validation.js';

//submit form
const listCart = JSON.parse(window.localStorage.getItem('cart'));
if (!listCart || listCart.length==0) {
    window.location.href='/'
}
// Get the modal
var modal = document.getElementById("myModal");

// Get the button that opens the modal
var btn = document.getElementById("myBtn");

// Get the <span> element that closes the modal
var span = document.getElementsByClassName("close")[0];

$('#save_btn').click(() => {
    let checkEmpty = validation.checkRequired([fullname, email, phone, address]);
    let checkEmailInvalid = validation.checkEmail(email);
    let checkPhoneInvalid = validation.checkNumberPhone(phone);
    let checkAddressInvalid = validation.checkAddress([city, district, ward]);

    if (
        checkEmpty &&
        checkEmailInvalid &&
        checkPhoneInvalid &&
        !checkAddressInvalid
    ) {
        var dsChiTietDH = []
        listCart.forEach((el) => {
            let converNumberAmount = Number(el.amount);
            var productItem = {
                idProduct: el.idFood,
                qty: converNumberAmount,
                price: el.price,
                nameProduct: el.title,
                attributes: el.size,
                imageProduct: el.img,
                attributeId: el.attributeId,
                attributeValueId: el.attributeValueId,
            }
            dsChiTietDH.push(productItem);
        })

     

        let thanhpho = $("#province option:selected").text()
        let quan = $("#village option:selected").text()
        let phuong = $("#district option:selected").text()
        let diachi = address.value + ', ' + quan + ', ' + phuong + ', ' + thanhpho
        let dienthoai = Number(phone.value)
       

        $.ajax('/checkout/HandleOrder', {
            data: {
               
                customerName: fullname.value,
                email: email.value,
                phone: dienthoai,
                address: diachi,
                province: thanhpho,
                district: phuong,
                ward: quan,
                listProduct: dsChiTietDH,
            },
            dataType: 'json',
            method: 'Post',
            success: function (res) {
                if (res.success) {
                    modal.style.display = "block";
                    $('#showDataModal').html("Ðặt hàng thành công, đơn hàng của bạn sẽ sớm được giao")
                    window.localStorage.removeItem('cart');
                    window.location.replace('/home') 
                }
                else {
                    console.log(res.err);
                    console.log(res.mess);
                    modal.style.display = "block";
                    $('#showDataModal').html(res.mess)
                }
             
          
             
            }
        })
    }
})



  



// When the user clicks on <span> (x), close the modal
span.onclick = function () {
    modal.style.display = "none";
}

// When the user clicks anywhere outside of the modal, close it
window.onclick = function (event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
}
