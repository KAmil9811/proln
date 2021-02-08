﻿import React, { Component } from "react";
import './SavedPrint.css';
import { ItemsTable } from '../saveProjectTables/ItemsTable'
import { GlassTableProject } from '../saveProjectTables/glassTableProject'
import Sidebar from '../Sidebar';

export class SavedPrint extends Component {
    constructor(props) {
        super(props);
        this.state = {
            value: '',
            value2: '',
            table: [],
            pieces: [],
            glass_ids: []

        }
    }

    componentDidMount() {
        const receiver = {
            order: {
                id_order: sessionStorage.getItem('orderId2'),
            },
            id: sessionStorage.getItem('cutId2'),
            user: {
                login: sessionStorage.getItem('login'),
            },
        }
           fetch(`api/Cut/Return_Porject`, {
            method: "post",
            body: JSON.stringify(receiver),
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
                
                var table2 = []
                
                sessionStorage.setItem('kolor', json[0].color)
                sessionStorage.setItem('grubosc', json[0].hight)
                sessionStorage.setItem('typ', json[0].type)
            })
    }


   

    //api/Cut/Save_Project
    saveProject = (event) => {
        event.preventDefault();
        this.props.history.push('/saved_projects')

    }
    generator = (event) => {
        
        const receiver = {
            order: {
                id_order: sessionStorage.getItem('orderId2'),
                color:sessionStorage.getItem('kolor'),
                thickness: sessionStorage.getItem('grubosc'),
                type:sessionStorage.getItem('typ'),
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
                console.log(json);
            })
        document.getElementById
    }

    cutOrder = (event) => {
        event.preventDefault();
        var id = sessionStorage.getItem('orderId2');
        sessionStorage.setItem('id_order', id);
        this.props.history.push('/pick_machine');
    }

    render() {
        let href = sessionStorage.getItem('login') + "_" + sessionStorage.getItem('orderId2') + "_" + sessionStorage.getItem('kolor') + "_" + sessionStorage.getItem('typ') + "_" + sessionStorage.getItem('grubosc')+ ".jpg"
        let href2 = sessionStorage.getItem('login') + "_" + sessionStorage.getItem('orderId2') + "_" + sessionStorage.getItem('kolor') + "_" + sessionStorage.getItem('typ') + "_" + sessionStorage.getItem('grubosc') +  ".pdf"
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
                <div >
                    <Sidebar />
                    <div className="title">
                        <h1 className="titletext">Cut project</h1>
                    </div>

                    <h3>{sessionStorage.getItem('uncat')}</h3>
                    <div className="table2">
                        <h2>Glasses</h2>
                        <GlassTableProject/>
                    </div>
                    <div className="table3">
                        <h2>Products</h2>
                        <ItemsTable/>
                        <img src={href} />
                        <div>
                            <button className="success_test" onClick={this.cutOrder}>cut</button>
                            <a href={href2} download><button className="success_test" onClick={this.generator} >Generate PDF </button></a>
                        </div>
                    </div>
                </div>
            );
        }
        else {

            return (
                <div >
                    <Sidebar />
                    <div className="title">
                        <h1 className="titletext">Cut project</h1>
                    </div>
                    <div className="test_c">
                        <h3>{sessionStorage.getItem('uncat')}</h3>
                        <div className="table2">
                            <h2>Glasses</h2>
                            <GlassTableProject />
                        </div>
                        <div className="table3">
                            <h2>Products</h2>
                            <ItemsTable />
                        </div>
                    </div>
                </div>
            );
        }
       
    }
}