﻿import React, { Component } from 'react';
import { GlassHistoryTable } from './GlassHistoryTable';
import Sidebar from '../Sidebar';


export class GlassHistory extends Component {
    constructor(props) {
        super(props);
    }
    logOut = (event) => {
        event.preventDefault();
        sessionStorage.removeItem('email')
        sessionStorage.removeItem('login')
        sessionStorage.removeItem('name')
        sessionStorage.removeItem('password')
        sessionStorage.removeItem('surname')
        sessionStorage.removeItem('user')
        sessionStorage.removeItem('admin')
        sessionStorage.removeItem('superAdmin')
        sessionStorage.removeItem('manager')
        sessionStorage.removeItem('magazineManagement')
        sessionStorage.removeItem('machineManagement')
        sessionStorage.removeItem('orderManagement')
        sessionStorage.removeItem('cutManagement')
        this.props.history.push('/')
    }


    homePage = (event) => {
        this.props.history.push('/home')
    }

    render() {
        if (sessionStorage.getItem('valid') === '') {
            return (
                <div className="HomePage">
                    <h1>Log in to have access!</h1>
                    <button type="submit" className="success_login" onClick={this.goback} >Log in</button>
                </div>
            );
        }
        else if (sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true' || sessionStorage.getItem('admin') === 'true') {
            return (
                <div className="GlasssHistory">
                    <Sidebar />
                    <div className="title">
                        <h1 className="titletext">Magazine history</h1>
                    </div>
                    <div>

                        <div className="nav_mw">


                        </div>
                        <div className="conteiner_mw">

                            <GlassHistoryTable />
                        </div>
                    </div>
                </div>
            );
        }
        else {
            return (
                <div className="HomePage">
                    <h1>Check if you have perrmission to this panel</h1>
                    <button type="submit" className="success_login" onClick={this.goback2} >Back to home page</button>
                </div>
            );

        }
    }
}