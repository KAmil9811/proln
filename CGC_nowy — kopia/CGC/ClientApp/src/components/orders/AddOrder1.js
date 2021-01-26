import React, { Component } from "react";
import './AddOrder1.css'
import Sidebar from '../Sidebar';
import datepicker from 'js-datepicker'

export class AddOrderOne extends Component {
    displayName = AddOrderOne;
    constructor(props) {
        super(props);
        this.state = {
            value: '',
            data:'',
        }
    }

    componentDidMount() {
        var today = new Date();
        var dd = String(today.getDate()).padStart(2, '0');
        var mm = String(today.getMonth() + 1).padStart(2, '0'); 
        var yyyy = today.getFullYear();

        today = mm + '.' + dd + '.' + yyyy;

        this.setState({
           data: today
        })
    }

    nextPage = (event) => {
        const order = {
            client: this.refs.client.value,
            priority: this.refs.priority.value,
            deadline: this.refs.deadline.value,
        }
        if (this.refs.client.value === "" || this.refs.priority.value === "" || this.refs.deadline.value === "") {
            alert("Uzupełnij dane")
        }
        else {
        sessionStorage.setItem('client', order.client)
        sessionStorage.setItem('prioryty', order.priority)
        sessionStorage.setItem('deadline', order.deadline)

            this.props.history.push('/addordersecond')
        }
    }

    cancelAdding = (event) => {
        this.props.history.push('/orderwarehouse')
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
        else if (sessionStorage.getItem('orderManagement') === 'true' || sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true' || sessionStorage.getItem('admin') === 'true') {
            return (

                <div>
                    <Sidebar />
                    <div className="title">
                        <h1 className="titletext">Order</h1>
                    </div>
                    <div className="AddOrder1">
                        <form>
                            <div className="form-group">
                                <label>Client</label>
                                <input
                                    type="text"
                                    name="client"
                                    className="form-control"
                                    id="inputClient"
                                    placeholder="Enter client name"
                                    ref="client"
                                />
                            </div>
                            <div className="form-group">
                                <label>Priority</label>
                                <input
                                    type="number"
                                    min="1"
                                    max="5"
                                    className="form-control"
                                    id="inputLogin"
                                    placeholder="Enter number from 1 to 5 ( 1 is the highest priority )"
                                    ref="priority"
                                />
                            </div>
                            <div className="form-group">
                                <label for="start">Deadline</label>
                                <input id="start"
                                    type="date"
                                    className="form-control"
                                    id="inputDeadline"
                                    ref="deadline"
                                    min="23-01-2020"
                                    max="23-01-2020"
                                />
                            </div>
                            <div className="form-group">
                                <button type="button" className="danger_add_order" onClick={this.cancelAdding}>Cancel</button>
                                <button type="button" className="success_add_order" onClick={this.nextPage}>Go next</button>

                            </div>
                        </form>
                    </div>

                </div>
            )
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