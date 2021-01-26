import React, { Component } from 'react';
import { MDBDataTable } from 'mdbreact';
import { MDBDataTableV5 } from 'mdbreact';
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
            ids: '',
            send:[],
        };
    }

   
   


    componentDidMount() {
        var table2 = [];
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
                    var j = i;
                    var deleted = '';
                    if (json[i].deleted === false) {
                        deleted === 'Send' || 'In magazine' || 'Send to magazine'
                    }
                    else {
                        deleted = 'Usunięty'
                    }
                    if (sessionStorage.getItem('magazineManagement') === 'true' || sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true' || sessionStorage.getItem('admin') === 'true') {
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
                                    }>Edit</button>
                                </Link>,
                            choice: <input type="checkbox" id={'check' + i} className={i} onClick={(e) => { this.check(e.target.id, table2[e.target.className].id, i) }} />,
                        })
                    }
                    else {
                        table2.push({
                            id: json[i].id,
                            id_item: json[i].id_item,
                            owner: json[i].owner,
                            status: json[i].status,
                            desk: json[i].desk,
                            
                        })
                    }
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
                                label: 'Owner',
                                field: 'owner',
                                sort: 'asc',
                                width: 150
                            },
                            {
                                label: 'Shelf',
                                field: 'desk',
                                sort: 'asc',
                                width: 150
                            },
                            {
                                label: 'Status',
                                field: 'status',
                                sort: 'asc',
                                width: 150
                            },


                            /*{
                                label: 'Edytuj',
                                field: 'edit',
                                sort: 'asc',
                                width: 150
                            },*/
                            {
                                label: '',
                                field: 'choice',
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
            <MDBDataTableV5


                hover
                entriesOptions={[10, 20, 50, 100]}
                entries={15}
                pagesAmount={10}
                data={this.state.table}
                searchTop


                materialSearch
                searchBottom={false}
                // barReverse
                //  pagingTop
                // scrollX
                // scrollY
                responsive
                // maxHeight="35vh"
                bordered



                //   maxHeight="20vh"
                // borderless
                // btn
                // dark


                //maxHeight="400px"

                // paginationLabel={["<", ">"]}

                sortable


                // small
                // tego w ciemnym trybie nie ruszać/ striped/
                // theadColor="indigo"
                theadTextWhite
                // theadColor="indigo"
                theadTextWhite
                // barReverse
                // className="User_table"
                // noBottomColumns
                sortable
            //info={false}


            //   autoWidth


            />
        )
    }


    check(number, id, miejsce) {
        var checkBox = document.getElementById(number);
        var arr = this.state.send
        if (checkBox.checked == true) {
            //alert('dodane' + '' + number)
            arr.push(id) 
            this.setState.send = arr
            console.log('Table' + '---' + this.state.send)
            
        } else {
            //alert('usunięte' + '' + number)
            const index = arr.indexOf(id);
            if (index > -1) {
                arr.splice(index, 1);
            }
            this.setState.send = arr
            console.log('Table' + '---' + this.state.send)
        }
    };

    sendId = (event) => {
        event.preventDefault();
        const receiver = {
            user: { login: sessionStorage.getItem('login') },
            product_id: this.state.send,

        }
        fetch(`api/Product/Released_Product`, {
            method: "post",
            body: JSON.stringify(
                receiver
            ),
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
                console.log(json)
                return (json)
            })
            .then(json => {
                alert('Products:' + ' ' + this.state.send + ' ' + 'have been sended')
            })
            .then(json => {
                window.location.reload();
            })
    }

    delete = (event) => {
        event.preventDefault();
        const receiver = {
            product_id: this.state.send,
            user: {
                login: sessionStorage.getItem('login'),
            }
        }
        console.log(receiver)

        
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
                    alert("You deleted product")
                })
                /*.then(json => {
                    window.location.reload();
                })*/
      


    }

    render() {
        let xd = this.table();
        if (sessionStorage.getItem('magazineManagement') === 'true' || sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true' || sessionStorage.getItem('admin') === 'true') {
            return (

                <div>
                    <div className="ready_glass_table_conteiner">
                        <button className="success_ready_glass_warehouse" onClick={this.sendId}>Sent selected to magazine</button>

                        <button className="danger_ready_glass_warehouse" onClick={this.delete}>Delete selected </button>

                    </div>
                    <div className="ready_glass_table_t_cointeiner">
                        {xd}
                    </div>

                </div>
            )
        }
        else {
                return (
                    <div>
                        <div className="ready_glass_table_t_cointeiner">
                            {xd}
                        </div>
                    </div>
                )
        }
    }

}