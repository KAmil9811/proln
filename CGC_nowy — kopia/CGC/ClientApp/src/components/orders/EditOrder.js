import React, { Component } from "react";
import './EditOrder.css';
import Sidebar from '../Sidebar';

export class EditOrder extends Component {
    displayName = EditOrder.name;
    constructor(props) {
        super(props);
        this.state = {
            value: '',
        }
    }

    handleEdit = (event) => {
        event.preventDefault();
        const receiver = {
            order: {
                deadline: this.refs.deadline.value,
                owner: this.refs.klient.value,
                priority: sessionStorage.getItem('priority'),
                id_order: sessionStorage.getItem('orderId'),
            },
            user: {
                login: sessionStorage.getItem('login'),
            }
        }

        console.log(receiver);
    }


    handleEditOrder = (event) => {
        event.preventDefault();
        const receiver = {
            order: {
                deadline: this.refs.deadline.value,
                owner: this.refs.klient.value,
                priority: this.refs.priority.value,
                id_order: sessionStorage.getItem('orderId'),
            },
            user: {
                login: sessionStorage.getItem('login'),
            }
        }

        if (this.refs.deadline.value === '') {
            alert("Podaj deadline");
        }
        else {

        fetch(`api/Order/Edit_Order`, {
            method: "post",
            body: JSON.stringify(receiver),
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
                console.log(receiver)
                console.log(json)
                return (json)
            })
        sessionStorage.setItem('klient', this.refs.klient.value);
        sessionStorage.setItem('priority', this.refs.priority.value);
        sessionStorage.setItem('deadline', this.refs.deadline.value);
            this.props.history.push('/oneorder');
        }
    }
    cancelEditOrder = (event) => {
        //dane z cache usuwają się przy wyjściu ze zlecenia
        sessionStorage.removeItem('orderId');
        sessionStorage.removeItem('klient');
        sessionStorage.removeItem('deadline');
        sessionStorage.removeItem('priority');  
        this.props.history.push('/orderwarehouse');
        
    }
    render() {
        return (
            <div>
                <Sidebar />
                    <div className="userChange">
                        <form>
                            <div className="form-group">
                                <h2>Edycja danych</h2>
                                <label>Zleceniodawca</label>
                                <input
                                    type="text"
                                    className="form-control"
                                    id="inputKlient"
                                    placeholder={sessionStorage.getItem('klient')}
                                    defaultValue={sessionStorage.getItem('klient')}
                                    ref="klient"
                                />
                            </div>
                            <div className="form-group">
                                <label>Priorytet</label>
                                <input
                                    type="text"
                                    className="form-control"
                                    id="inputPriority"
                                    placeholder={sessionStorage.getItem('priority')}
                                    defaultValue={sessionStorage.getItem('priority')}
                                    ref="priority"
                                />
                            </div>
                            <div className="form-group">
                                <label>Deadline</label>
                                <input
                                    type="date"
                                    className="form-control"
                                    id="inputWidth"
                                    //placeholder={sessionStorage.getItem('deadline')}
                                    //defaultValue={sessionStorage.getItem('deadline')}
                                    ref="deadline"
                                />
                            </div>
                            <div className="form-group">
                                <button type="submit" className="danger_edit_order" onClick={this.cancelEditOrder}>Anuluj</button>
                                <button type="submit" className="succes_edit_order" onClick={this.handleEditOrder}>Zastosuj zmiany</button>
                            </div>

                        </form>
                </div>


            </div>

        );
    }
}