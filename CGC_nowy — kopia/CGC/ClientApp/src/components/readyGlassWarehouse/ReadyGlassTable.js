import React, { Component } from 'react';
import { MDBDataTable } from 'mdbreact';
import './ReadyGlassTable.css'
import { Link } from 'react-router-dom';


export class ReadyGlassTable extends Component {
    constructor(props) {
        super(props);
        this.state = {
            table: {
                columns: [],
                rows: []
            },
            ids:'',
        };
    }



    componentDidMount() {
        var table2 = [];
        var tableIds = [];
        fetch(`api/Product/Get_Products`, {
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
                console.log(json.length);
                console.log(json);
                for (var i = 0; i < json.length; i++) {
                    var deleted = '';
                    if (json[i].deleted === false) {
                        deleted === 'Send' || 'In magazine' ||'Send to magazine'
                    }
                    else {
                        deleted = 'Usunięty'
                    }
                    table2.push({
                        id: json[i].id,
                        id_item: json[i].id_item,
                        owner: json[i].owner,
                        status: json[i].status,
                        desk: json[i].desk,
                        edit:
                            <Link to="/product_edit"><button className="delete1" id={i}
                                onClick={
                                    (e) => {
                                        //console.log(e.target.id)
             
                                        sessionStorage.setItem('owner', json[e.target.id].owner);
                                        sessionStorage.setItem('status', json[e.target.id].status);
                                        sessionStorage.setItem('desk', json[e.target.id].desk);
                                        sessionStorage.setItem('id', JSON.stringify(json[e.target.id].glass_id));
                                    }
                                }>Edytuj</button>
                            </Link>,
                        del: <button className="delete" id={i} onClick={(e) => { this.delete(table2[e.target.id].id, table2[e.target.id].status) }}> Usuń  </button> 
     
                           
                    })
                };
                this.setState({
                    table: {
                        columns: [
                            
                         
                            {
                                label: 'ID',
                                field: 'id',
                                sort: 'asc',
                                width: 150
                            },
                            {
                                label: 'ID Item',
                                field: 'id_item',
                                sort: 'asc',
                                width: 150
                            },
                          
                            {
                                label: 'Właściciel',
                                field: 'owner',
                                sort: 'asc',
                                width: 150
                            },
                            {
                                label: 'Półka',
                                field: 'desk',
                                sort: 'asc',
                                width: 150
                            },
                            {
                                label: 'Status_product',
                                field: 'status',
                                sort: 'asc',
                                width: 150
                            },
                            
                           
                            {
                                label: 'Edytuj',
                                field: 'edit',
                                sort: 'asc',
                                width: 150
                            },
                            {
                                label: 'Usuń',
                                field: 'del',
                                sort: 'asc',
                                width: 150
                            }
                        ],
                        rows: table2
                    }
                });
            })
    };

    table() {
        return (
            <MDBDataTable
                /*striped*/
                bordered
                small
                data={this.state.table}
            />
        )
    }
    delete(product, deleted) {
        const receiver = {
            product: {
                id: product,
            },
            user: {
                login: sessionStorage.getItem('login'),
            }
        }
        //console.log(receiver)

        if (deleted === 'Send' || 'In magazine' || 'Send to magazine') {
            fetch(`api/Product/Delete_Product`, {
                method: "post",
                body: JSON.stringify(receiver),
                headers: {
                    'Content-Type': 'application/json'
                }
            })

                .then(res => res.json())
                .then(json => {
                    console.log(json)
                    return (json);
                })
                .then(json => {
                    alert("Usunięto produkt")
                })
                .then(json => {
                    window.location.reload();
                })
        }
        else {
            alert('Nie możesz usunąć tego przedmiotu!')
        }


    }

    render() {
        let xd = this.table();
        return (

            <div>
                {xd}
            </div>
        )
    }

}