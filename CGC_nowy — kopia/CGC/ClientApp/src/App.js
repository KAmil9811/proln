import React, { Component } from 'react';
import { Route } from 'react-router';
import { Login } from './components/Login';
import { ControlPanel } from './components/controlPanel/ControlPanel';
import { AddAcount } from './components/controlPanel/AddAcount';
import { HomePage } from './components/homePage/HomePage';
import { GlassWarehouse } from './components/glassWarehouse/GlassWarehouse';
import { MachineWarehouse } from './components/machineWarehouse/MachineWarehouse';
import { PasswordChange } from './components/changeOfThings/PasswordChange';
import { EmailChange } from './components/changeOfThings/EmailChange';
import { UserChanging } from './components/changeOfThings/UserChanging';
import { OrderWarehouse } from './components/orders/OrderWarehouse';
import { AddGlass } from './components/addGlass/AddGlass';
import { AddOrderOne } from './components/orders/AddOrder1';
import { AddOrderTwo } from './components/orders/AddOrder2';
import { OneOrder } from './components/orders/OneOrder';
import { AddCutMachine } from './components/addCutMachine/AddCutMachine';
import { AddTypeMachine } from './components/addCutMachine/AddTypeMachine';
import { PasswordReset1 } from './components/passwordReset/PasswordReset1';
import { PasswordReset2 } from './components/passwordReset/PasswordReset2';
import { AddType } from './components/addGlass/AddType';
import { AddColor } from './components/addGlass/AddColor';
import { GlassEdit } from './components/glassEdit/GlassEdit';
import { GlassAtributes } from './components/machineAndGlass/glassAttributes';
import { CutMachineEdit } from './components/machineAndGlass/cutMachineEdit';
import { GlassColorEdit } from './components/machineAndGlass/glassColorEdit';
import { GlassTypeEdit } from './components/machineAndGlass/glassTypeEdit';
import { MachineTypeEdit } from './components/machineAndGlass/editTypeMachine';
import { ReadyGlassWarehouse } from './components/readyGlassWarehouse/ReadyGlassWarehouse';
import { EditOrder } from './components/orders/EditOrder';
import { EditOrderItem } from './components/orders/EditOrderItem';
import { EditProduct } from './components/readyGlassWarehouse/EditProduct';

import { Test } from './components/opti/test';

import { SelectionOfOrders } from './components/Production/SelectionOfOrders';
import { ReadyPackages } from './components/Production/ReadyPackages';


export default class App extends Component {
    displayName = App.name

    render() {
        return (
            <div>
                <Route exact path='/' component={Login} />
                <Route path='/controlpanel' component={ControlPanel} />
                <Route path='/change_password' component={PasswordChange} />
                <Route path='/change_email' component={EmailChange} />
                <Route path='/user_change' component={UserChanging} />
                <Route exact path='/add_acount' component={AddAcount} />
                <Route path='/home' component={HomePage} />
                <Route path='/glasswarehouse' component={GlassWarehouse} />
                <Route path='/machinewarehouse' component={MachineWarehouse} />
                <Route path='/add_glass' component={AddGlass} />
                <Route path='/add_type' component={AddType} />
                <Route path='/add_color' component={AddColor} />
                <Route path='/addorderfirst' component={AddOrderOne} />
                <Route path='/addordersecond' component={AddOrderTwo} />
                <Route path='/oneorder' component={OneOrder} />
                <Route path='/orderwarehouse' component={OrderWarehouse} />
                <Route path='/add_cutmachine' component={AddCutMachine} />
                <Route path='/add_typemachine' component={AddTypeMachine} />
                <Route path='/reset_password1' component={PasswordReset1} />
                <Route path='/reset_password2' component={PasswordReset2} />
                <Route path='/glass_edit' component={GlassEdit} />
                <Route path='/glassatibutes' component={GlassAtributes} />
                <Route path='/cutmachineedit' component={CutMachineEdit} />
                <Route path='/glasscoloredit' component={GlassColorEdit} />
                <Route path='/glasstypeedit' component={GlassTypeEdit} />
                <Route path='/machinetypeedit' component={MachineTypeEdit} />
                <Route path='/ready_glass_warehouse' component={ReadyGlassWarehouse} />
                <Route path='/edit_order' component={EditOrder} />
                <Route path='/edit_order_item' component={EditOrderItem} />
                <Route path='/product_edit' component={EditProduct} />

                <Route path='/test' component={Test} />

                <Route path='/selection_of_orders' component={SelectionOfOrders} />
                <Route path='/ready_packages' component={ReadyPackages} />
                


            </div>
        );
    }
}
