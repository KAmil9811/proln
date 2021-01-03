import React, { Component } from 'react';
import { GlassTable } from './GlassTable';
import './GlassWarehouse.css'
import Sidebar from '../Sidebar';

export class GlassWarehouse extends Component {
    displayName = GlassWarehouse.name;
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

    addGlass = (event) => {
        this.props.history.push('/add_glass')
    }

    historyGlass = (event) => {
        this.props.history.push('/glass_history')
    }

    delGlass = (event) => {
        var amount = prompt("Podaj Id szkła które chcesz usunąć:");
        var amount2 = parseInt(amount)
        sessionStorage.setItem('idg', amount)
        console.log(amount)
        if (amount === null) {
            return;
        }
        else if (isNaN(amount2)) {
            alert("Proszę wprowadzić liczbę!");
        }
        else {
            const receiver = {
                user: {
                    login: sessionStorage.getItem('login')
                },
                id_glass: sessionStorage.getItem('idg')
            }

            fetch(`api/Magazine/Remove_Glass`, {
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
                    if (json[0].error_Messege === "Glass_Id_no_exist") {
                        alert("Szkło o podanym id nie istnieje")
                    }
                    else {
                        alert("Usunięto szkło o id:" + amount2)
                        window.location.reload();
                    }
                    
                })
            .then(json => {
            window.location.reload();
        })
        }

       
    }

    readyGlassWarehouse = (event) => {
        this.props.history.push('/ready_glass_warehouse')
    }

    history() {
        if (sessionStorage.getItem('admin') === 'true' || sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true') {
            return (
                <button type="button" className="add_glass" onClick={this.historyGlass}>Historia szkła</button>
            )
        }
    }
    render() {
        let history = this.history();
        return (
            <div>

                <Sidebar />
                <div className="conteiner_gw">
                    <button type="button" className="del_glass_x" onClick={this.delGlass}>Usuń szkło</button>
                    <button type="button" className="add_glass" onClick={this.addGlass}>Dodaj szkło</button>
                    {history}

                    <div className="tableglass">
                        <GlassTable />
                    </div>
                </div>
            </div>
        );
    }
}
