import React, { Component } from 'react';
import { PackagesTable } from './PackagesTable';
import Sidebar from '../Sidebar';
import './ReadyPackages.css'



export class ReadyPackages extends Component {
    displayName = ReadyPackages.name;
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

    choosePackage = (event) => {
        this.props.history.push('/ready_packages')
    }

    goback = (event) => {
        this.props.history.push('/')
    }
    goback2 = (event) => {
        this.props.history.push('/home')
    }

    render() {
        if (sessionStorage.getItem('valid') === '') {
            return (
                <div className="HomePageFail">
                    <h1>Log in to have access!</h1>
                    <button type="submit" className="success_login2" onClick={this.goback} >Log in</button>
                </div>
            );
        }
        else {
            return (
                <div className="ReadyPackages">
                    <Sidebar />
                    <div className="title">
                        <h1 className="titletext">Select package</h1>
                    </div>
                    <div className="ready_packages_conteiner">


                        <PackagesTable />

                    </div>


                </div>
            );
        }
    }
}