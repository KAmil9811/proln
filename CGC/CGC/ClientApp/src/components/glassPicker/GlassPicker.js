import React, { Component } from 'react';
import { MDBDataTable } from 'mdbreact';
import { MDBDataTableV5 } from 'mdbreact';
import { Link } from 'react-router-dom';
import './GlassTable.css';
import Sidebar from '../Sidebar';
import { Route, withRouter } from 'react-router-dom';

import { history } from 'react-router-dom';


export class GlassPicker extends Component {
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
        var table2 = [];
        var tableIds = [];
        fetch(`api/Magazine/Return_All_Glass`, {
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {

                for (var i = 0; i < json.length; i++) {
                    if (sessionStorage.getItem('magazineManagement') === 'true' || sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true' || sessionStorage.getItem('admin') === 'true') {
                        table2.push({
                            length: json[i].length,
                            width: json[i].width,
                            thickness: json[i].hight,
                            color: json[i].color,
                            type: json[i].type,
                            owner: json[i].owner,
                            desk: json[i].desk,
                            choice: <input type="checkbox" id={'check' + i} className={i} onClick={(e) => { this.check(e.target.id, table2[e.target.className].id, i) }} />,
                            id: '',
                            edit:
                                <Link to="/glass_edit"><button className="info_t" id={i}
                                    onClick={
                                        (e) => {
                                            //console.log(e.target.id);
                                            sessionStorage.setItem('length', json[e.target.id].length);
                                            sessionStorage.setItem('width', json[e.target.id].width);
                                            sessionStorage.setItem('thickness', json[e.target.id].hight);
                                            sessionStorage.setItem('color', json[e.target.id].color);
                                            sessionStorage.setItem('type', json[e.target.id].type);
                                            //sessionStorage.setItem('amount', json[e.target.id].length);
                                            sessionStorage.setItem('owner', json[e.target.id].owner);
                                            sessionStorage.setItem('desk', json[e.target.id].desk);
                                            sessionStorage.setItem('id', JSON.stringify(json[e.target.id].glass_Id));
                                        }
                                    }>Edit</button>
                                </Link>

                        })
                    }
                    else {
                        table2.push({
                            length: json[i].length,
                            width: json[i].width,
                            thickness: json[i].hight,
                            color: json[i].color,
                            type: json[i].type,
                            owner: json[i].owner,
                            desk: json[i].desk,
                            id: '',
                        })

                    }
                };

                for (var k = 0; k < table2.length; k++) {
                    var amount = json[k].length;
                    //console.log(amount)
                    for (var j = 0; j < amount; j++) {
                        table2[k].id = json[k].glass_Id
                    };
                }

                if (sessionStorage.getItem('magazineManagement') === 'true' || sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true' || sessionStorage.getItem('admin') === 'true') {
                    this.setState({
                        table: {
                            columns: [
                                {
                                    label: 'Length',
                                    field: 'length',
                                    sort: 'asc',
                                    width: 150
                                },
                                {
                                    label: 'Width',
                                    field: 'width',
                                    sort: 'asc',
                                    width: 150
                                },
                                {
                                    label: 'Thickness',
                                    field: 'thickness',
                                    sort: 'asc',
                                    width: 150
                                },
                                {
                                    label: 'Color',
                                    field: 'color',
                                    sort: 'asc',
                                    width: 150
                                },
                                {
                                    label: 'Type',
                                    field: 'type',
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
                                    label: 'Ref number',
                                    field: 'id',
                                    sort: 'asc',
                                    width: 150
                                },
                                {
                                    label: 'Edit',
                                    field: 'edit',
                                    sort: 'asc',
                                    width: 150
                                },
                                {
                                    label: '',
                                    field: 'choice',
                                    /*sort: 'asc',*/
                                    width: 150
                                }

                                /* {
                                     label: 'Usuń',
                                     field: 'action',
                                     sort: 'asc',
                                     width: 150
                                 },*/

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
                                    width: 150
                                },
                                {
                                    label: 'Width',
                                    field: 'width',
                                    sort: 'asc',
                                    width: 150
                                },
                                {
                                    label: 'Thickness',
                                    field: 'thickness',
                                    sort: 'asc',
                                    width: 150
                                },
                                {
                                    label: 'Color',
                                    field: 'color',
                                    sort: 'asc',
                                    width: 150
                                },
                                {
                                    label: 'Type',
                                    field: 'type',
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
                                    label: 'ref number',
                                    field: 'id',
                                    sort: 'asc',
                                    width: 150
                                },
                            ],
                            rows: table2
                        }
                    });
                }
            })
    };

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


    pick = (event) => {
        event.preventDefault();
        /*const receiver = {
            user: {
                login: sessionStorage.getItem('login')
            },
            glass_Id: this.state.send
        }

        fetch(`api/Magazine/Remove_Glass`, {
            method: "post",
            body: JSON.stringify(receiver),
            headers: {
                'Authorization': 'Bearer ' + sessionStorage.getItem('token'),
                'Content-Type': 'application/json'
            }
        })

            .then(res => res.json())
            .then(json => {

                return (json);
            })
            .then(json => {

                alert("You deleted selected glass")
                window.location.reload();


            })*/
    }

    render() {
        let xd = this.table();
        return (

            <div>
                <div className="glass_table_b">
                    <button className="danger_glas_magazine" onClick={this.pick}> Pick selected </button>

                </div>

                {xd}


            </div>
        )
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



}
