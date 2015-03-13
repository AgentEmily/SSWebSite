function addListItem() {
    
     var write = $('#newItem').val();
     var list = $('#itemList');
     var item = $('<li><button class="checkbox">&#x2713;</button><span class="list">' + write + '</span><button class="delete">X</button></li>');
     var newItem = $("#newItem");

     if (write.length === 0  || write.length > 33) {
        return false;
    }
    
    list.append(item);
    $(newItem).val('');
}

function deleteItem() {

    $(this).parent().remove();
}

function dimItem() {

    $(this).parent().toggleClass('dim');
}

$(function() {

    var add = $('#addItem');
    var newItem = $('#newItem');
    var list = $('#itemList');
    
    add.on('click', addListItem);
    list.on('click', '.checkbox', dimItem);
    list.on('click', '.delete', deleteItem);
    newItem.on('keypress', function (e) {
        if (e.which == 13) {
            addListItem();
        }
    });
});

