import React, { Component } from 'react';
import { MDBDataTable } from 'mdbreact';
import { MDBDataTableV5 } from 'mdbreact';
import { Link } from 'react-router-dom';
import Sidebar from '../Sidebar';
import './OneOrderTable.css';

export class OneOrderTable extends Component {
    constructor(props) {
        super(props);
        this.state = {
            table: {
                columns: [],
                rows: []
            },

            ids: '',
            send: [],
        };
    }



    componentDidMount() {
        const receiver = {
            order: { id_order: sessionStorage.getItem('orderId') }
        }
        fetch(`api/Order/Return_All_Items`, {
            method: "post",
            body: JSON.stringify(receiver),
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
               
                return (json)
            })
            .then(json => {
                var table2 = [];
                for (var i = 0; i < json.length; i++) {
                    if (sessionStorage.getItem('orderManagement') === 'true' || sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true' || sessionStorage.getItem('admin') === 'true') {
                        table2.push({
                            length: json[i].length,
                            width: json[i].width,
                            thickness: json[i].thickness,
                            color: json[i].color,
                            type: json[i].type,
                            ids: json[i].id,
                            status: json[i].status,
                            desk: json[i].desk,
                            more: <Link to="/edit_order_item"><button className="prim_t" id={i}
                                onClick={
                                    (e) => {
                                        /* console.log(table2[e.target.id].items);*/
                                        sessionStorage.setItem('thickness', table2[e.target.id].thickness);
                                        sessionStorage.setItem('length', table2[e.target.id].length);
                                        sessionStorage.setItem('width', table2[e.target.id].width);
                                        sessionStorage.setItem('color', table2[e.target.id].color);
                                        sessionStorage.setItem('type', table2[e.target.id].type);
                                        sessionStorage.setItem('status', table2[e.target.id].status);
                                        sessionStorage.setItem('desk', table2[e.target.id].desk);
                                        sessionStorage.setItem('itemId', table2[e.target.id].ids);
                                    }
                                }>Edit glass</button></Link>,
                            choice: <input type="checkbox" id={'check' + i} className={i} onClick={(e) => { this.check(e.target.id, table2[e.target.className].ids, i) }} />,
                        })
                    }
                    else {
                        table2.push({
                            length: json[i].length,
                            width: json[i].width,
                            thickness: json[i].thickness,
                            color: json[i].color,
                            type: json[i].type,
                            ids: json[i].id,
                            status: json[i].status,
                            desk: json[i].desk,
                           
                        })
                        }
                }
                if (sessionStorage.getItem('orderManagement') === 'true' || sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true' || sessionStorage.getItem('admin') === 'true') {
                    this.setState({
                        table: {
                            columns: [
                                {
                                    label: 'Length',
                                    field: 'length',
                                    sort: 'asc',
                                    width: 30
                                },
                                {
                                    label: 'Width',
                                    field: 'width',
                                    sort: 'asc',
                                    width: 30
                                },
                                {
                                    label: 'Thickness',
                                    field: 'thickness',
                                    sort: 'asc',
                                    width: 30
                                },
                                {
                                    label: 'Color',
                                    field: 'color',
                                    sort: 'asc',
                                    width: 30
                                },
                                {
                                    label: 'Type',
                                    field: 'type',
                                    sort: 'asc',
                                    width: 30
                                },
                                {
                                    label: 'Id',
                                    field: 'ids',
                                    sort: 'asc',
                                    width: 30
                                },
                                {
                                    label: 'Status',
                                    field: 'status',
                                    sort: 'asc',
                                    width: 30
                                },
                                {
                                    label: 'Edit',
                                    field: 'more',
                                    sort: 'asc',
                                    width: 30
                                },
                                {
                                    label: '',
                                    field: 'choice',
                                    sort: 'asc',
                                    width: 30
                                },
                            ],
                            rows: table2
                        }
                    });
                }
                else {
                    this.setState({
                        table: {
                            columns: [
                                {
                                    label: 'Length',
                                    field: 'length',
                                    sort: 'asc',
                                    width: 30
                                },
                                {
                                    label: 'Width',
                                    field: 'width',
                                    sort: 'asc',
                                    width: 30
                                },
                                {
                                    label: 'Thickness',
                                    field: 'thickness',
                                    sort: 'asc',
                                    width: 30
                                },
                                {
                                    label: 'Color',
                                    field: 'color',
                                    sort: 'asc',
                                    width: 30
                                },
                                {
                                    label: 'Type',
                                    field: 'type',
                                    sort: 'asc',
                                    width: 30
                                },
                                {
                                    label: 'Id',
                                    field: 'ids',
                                    sort: 'asc',
                                    width: 30
                                },
                                {
                                    label: 'Status',
                                    field: 'status',
                                    sort: 'asc',
                                    width: 30
                                },
                            ],
                            rows: table2
                        }
                    });
                }
            })
    }


    check(number, id, miejsce) {
        var checkBox = document.getElementById(number);
        var arr = this.state.send
        if (checkBox.checked == true) {
            //alert('dodane' + '' + number)
            arr.push(id)
            this.setState.send = arr
            

        } else {
            //alert('usunięte' + '' + number)
            const index = arr.indexOf(id);
            if (index > -1) {
                arr.splice(index, 1);
            }
            this.setState.send = arr
            
        }
    };


    sendId = (event) => {
        event.preventDefault();
        const receiver = {
            user: {
                company: sessionStorage.getItem('company'),
                login: sessionStorage.getItem('login')
            },
            item_Id: this.state.send,
            order: { id_order :sessionStorage.getItem('orderId') },
        }

        fetch(`api/Order/Remove_Item`, {
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
             
                return (json)
            })
            .then(json => {
                alert('Items with id:' + ' ' + this.state.send + ' ' + ' has been deleted')
            })
            .then(json => {
                window.location.reload();
            })

      
    }

    kurwaaaaa = (event) => {
        event.preventDefault();
        
    }

    table() {
        return (
            <MDBDataTableV5


                data={this.state.table}
                hover
                entriesOptions={[10, 20, 50, 100]}
                entries={10}
                pagesAmount={10}
                searchTop
                materialSearch
                searchBottom={false}
                responsive
                bordered
                paginationLabel={["Previous", "Next"]}
                sortable
                // small
                theadTextWhite
                theadTextWhite
                className="table_corection"

            />
        )
    }

    render() {
        let table = this.table();
        if (sessionStorage.getItem('orderManagement') === 'true' || sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true' || sessionStorage.getItem('admin') === 'true') {
            return (

                <div>

                    <button className="danger_one_order_table" onClick={this.sendId}>Delete selected</button>
                    {table}
                </div>
            )

        }
        else {
            return (

                <div>

                    
                    {table}
                </div>
            )
        }
    }

}