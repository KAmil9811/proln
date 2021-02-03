import Sidebar from '../Sidebar';
import React, { Component } from 'react';
import { OptiWarehouseTable } from './optiWarehouseTable';


export class OptiWarehouse extends Component {
    displayName = OptiWarehouse.name;
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
                <div className="aaaaaaaa" >
                    <div className="phone">
                        <h1>No access on the phone</h1>
                    </div>
                    <div>

                        <Sidebar />
                        <div className="title">
                            <h1 className="titletext"> Saved projects</h1>
                        </div>
                        <div className="nav_mw">
                            <button className="add_machine" onClick={this.addMachine}>Optimize the new part</button>
                            <OptiWarehouseTable />

                        </div>
                        <div className="conteiner_mw">

                        </div>



                    </div >
                </div>
            );
        }
    }
}
