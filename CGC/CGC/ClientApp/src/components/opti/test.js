﻿import React, { Component } from "react";
import './test.css';
import { OptiTable } from './OptiTable'
import { OptiTableItems } from './OptiTableItems'
import Sidebar from '../Sidebar';
import { MDBDataTable } from 'mdbreact';
import { MDBDataTableV5 } from 'mdbreact';

export class Test extends Component {
    constructor(props) {
        super(props);
        this.state = {
            value: '',
            value2: '',
            table: [],
            pieces: [],
            glass_ids: [],
            position: '',
            table: {
                columns: [],
                rows: []
            },
            table2: {
                columns: [],
                rows: []
            },
        }
    }

    componentDidMount() {
        const receiver = {
            order: {
                id_order: sessionStorage.getItem('idOpti'),
                color: sessionStorage.getItem('colorOpti'),
                type: sessionStorage.getItem('typeOpti'),
                thickness: sessionStorage.getItem('thicknessOpti'),
            },
            user: {
                login: sessionStorage.getItem('login'),
            },
        }
        fetch(`api/Cut/Magic`, {
            method: "post",
            body: JSON.stringify(receiver),
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
             
                return(json)
            })
            .then(json => {
                var table3 = [];
                var table2 = [];
                sessionStorage.setItem('kolor', json[0].color)
                sessionStorage.setItem('typ', json[0].type)
                sessionStorage.setItem('grubosc', json[0].hight)
                for (var i = 0; i < json.length - 1; i++) {
                    table3.push({
                        length: json[i].length,
                        width: json[i].width,
                        thickness: json[i].hight,
                        color: json[i].color,
                        type: json[i].type,
                        ids: json[i].glass_info[0].id,
                        status: json[i].status,
                        desk: json[i].desk,


                    })
                };
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
                                label: 'No.',
                                field: 'ids',
                                sort: 'asc',
                                width: 30
                            },
                        ],
                        rows: table3
                    }
                });
                for (var i = 0; i < json.length-1; i++) {
                    for (var j = 0; j < json[i].glass_info.length; j++) {
                        for (var x = 0; x < json[i].glass_info[j].pieces.length; x++) {
                            table2.push({
                                length: json[i].glass_info[0].pieces[x].lenght,
                                width: json[i].glass_info[0].pieces[x].widht,
                                ids: json[i].glass_info[0].pieces[x].id,
                            })

                        }


                    }
                };
                this.setState({
                    table2: {
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
                                label: 'No.',
                                field: 'ids',
                                sort: 'asc',
                                width: 30
                            },
                        ],
                        rows: table2
                    }
                });
            })
            
    }


    cutOrder = (event) => {
        event.preventDefault();
        const receiver = {
            glasses:
                this.state.glass_ids
            ,
            order: {
                id_order: sessionStorage.getItem('idOpti')
            }

        }


        fetch(`api/Cut/Save_Project`, {
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
                sessionStorage.setItem('id_order', json);
            })
        this.props.history.push('/pick_machine');

       
    }

    saveProject = (event) => {
        event.preventDefault();
        const receiver = {
            glasses:
                this.state.glass_ids
            ,
            order: {
                id_order: sessionStorage.getItem('idOpti')
            },
            user: {
                login: sessionStorage.getItem('login'),
            },

        }


        fetch(`api/Cut/Save_Project`, {
            method: "post",
            body: JSON.stringify(receiver),
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
                
                sessionStorage.setItem('id_order', json.order.id_order);
                return (json)
            })
        //this.props.history.push('/glasswarehouse');

       

    }

    generator = (event) => {
        const receiver = {
            order: {
                id_order: sessionStorage.getItem('idOpti'),
                color:sessionStorage.getItem('colorOpti'),
                    type:sessionStorage.getItem('typeOpti'),
                thickness:sessionStorage.getItem('thicknessOpti'),
            },
            user: {
                login: sessionStorage.getItem('login'),
            },
        }
        fetch(`api/Cut/CreatePdf`, {
            method: "post",
            body: JSON.stringify(receiver),
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
               
            })
        document.getElementById
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
                sortable
                theadTextWhite
                theadTextWhite



            />
        )
    }

    table2() {
        return (
            <MDBDataTableV5



                data={this.state.table2}
                hover
                entriesOptions={[10, 20, 50, 100]}
                entries={10}
                pagesAmount={10}
                searchTop
                materialSearch
                searchBottom={false}
                responsive
                bordered
                sortable
                theadTextWhite
                theadTextWhite



            />
        )
    }


    render() {
        let table1 = this.table();
        let table2 = this.table2();
        let href = sessionStorage.getItem('login') + "_" + sessionStorage.getItem('orderId2') + "_" + sessionStorage.getItem('colorOpti') + "_" + sessionStorage.getItem('typeOpti') + "_" + sessionStorage.getItem('thicknessOpti') + ".jpg"
        let href2 = sessionStorage.getItem('login') + "_" + sessionStorage.getItem('orderId2') + "_" + sessionStorage.getItem('colorOpti') + "_" + sessionStorage.getItem('typeOpti') + "_" + sessionStorage.getItem('thicknessOpti') + ".pdf"
        if (sessionStorage.getItem('valid') === '') {
            return (
                <div className="HomePage">
                    <h1>Log in to have access!</h1>
                    <button type="submit" className="success_login" onClick={this.goback} >Log in</button>
                </div>
            );
        }
        else if (sessionStorage.getItem('cutManagement') === 'true' || sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true' || sessionStorage.getItem('admin') === 'true') {
            return (
                <div className="tescik" >
                    <Sidebar />
                    <div className="title">
                        <h1 className="titletext">Cut project</h1>
                    </div>

                    <h3>{sessionStorage.getItem('uncat')}</h3>
                    <div className="table2">
                        <h2>Glasses</h2>
                        {table1}
                    </div>
                    <div className="table3">
                        <h2>Products</h2>
                        {table2}
                        <img src={href}/>
                        <div>
                            <button className="prim_test" onClick={this.saveProject}>Save project</button>
                            <button className="success_test" onClick={this.cutOrder}>Save and cut</button>
                            <a href={href2} download><button className="success_test" onClick={this.generator} >Generate PDF </button></a>
                        </div>
                    </div>
                </div>
            );
        }
        else {

            return (
                <div className="tescik" >
                    <Sidebar />
                    <div className="title">
                        <h1 className="titletext">Cut project</h1>
                    </div>
                    <div className="test_c">
                        <h3>{sessionStorage.getItem('uncat')}</h3>
                        <div className="table2">
                            <h2>Glasses</h2>
                            {table1}
                        </div>
                        <div className="table3">
                            <h2>Products</h2>
                            {table2}
                        </div>
                    </div>
                </div>
            );
        }
    }
}

